using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private GUIStyle solidBoxStyle;

    public CharacterController controller;
    private PlayerStatus status;

    private MoveToPosition moveScript;

    private float gravity = -9.81f;
    public float moveSpeed = 2f;
    private float jumpHeight = 1;
    public float battleThreshold = 1f;
    Vector3 playerVelocity;
    Vector3 rotateDirection;
    float yVelocity = 0;

    private Animator animator;
    private MouseLook mouse;
    private MoveCamera cam;

    private GameObject currentEnemy;
    private EnemyController currentEnemyController;
    private EnemyStatus currentEnemyStatus;
    private GameObject currentArena;

    private GameObject[] enemies;
    private GameObject[] arenas;

    private bool isControllable = true;
    private bool isInBattle = false;

    private bool playerTurn = false;
    private bool executing = false;

    private bool gameOver = false;
    private float alpha = 0f;
    private GUIStyle textStyle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        status = GetComponent<PlayerStatus>();

        moveScript = GetComponent<MoveToPosition>();

        animator = GetComponentInChildren<Animator>();
        mouse = GetComponentInChildren<MouseLook>();
        cam = GetComponentInChildren<MoveCamera>();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        arenas = GameObject.FindGameObjectsWithTag("Arena");

        cam.enabled = false;
        mouse.enabled = true;
        moveScript.enabled = false;

        textStyle = new GUIStyle();
        textStyle.fontSize = 50;
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.normal.textColor = Color.red;
    }

    // Helper function to create a solid color texture
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Texture2D tex = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = col;
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    public bool IsControllable
    {
        get { return isControllable; }
        set { isControllable = value; }
    }

    public bool IsInBattle
    {
        get { return isInBattle; }
        set { isInBattle = value; }
    }

    GameObject CheckForEnemy()
    {
        float distance = Mathf.Infinity;
        GameObject closest = null;

        foreach (GameObject enemy in enemies)
        {
            if (!(enemy.GetComponent(typeof(EnemyStatus)) as EnemyStatus).isAlive())
                continue;
            Vector3 diff = enemy.transform.position - transform.position;
            float thisDistance = diff.sqrMagnitude;
            if (thisDistance < battleThreshold && thisDistance < distance)
            {
                closest = enemy;
                distance = thisDistance;
            }
        }

        return closest;
    }

    GameObject FindClosestArena()
    {
        float distance = Mathf.Infinity;
        GameObject closest = null;

        foreach(GameObject arena in arenas)
        {
            Vector3 diff = arena.transform.position - transform.position;
            float thisDistance = diff.sqrMagnitude;
            if (thisDistance < distance)
            {
                closest = arena;
                distance = thisDistance;
            }
        }

        return closest;
    }

    void SetActiveChildren(GameObject arena, bool value)
    {
        foreach (Transform t in arena.transform)
        {
            t.gameObject.SetActive(value);
        }
    }

    void OnGUI()
    {
        if (solidBoxStyle == null)
        {
            solidBoxStyle = new GUIStyle(GUI.skin.box);
            solidBoxStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 1f)); // Black, fully opaque
        }

        if (gameOver)
        {
            Color overlayColor = new Color(0, 0, 0, alpha);
            GUI.color = overlayColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);

            GUI.color = new Color(1, 0, 0, alpha);
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "GAME OVER", textStyle);
        }


        GUI.Box(new Rect(Screen.width - 100, Screen.height - 50, 100, 50),
            $"HP: {status.GetHealth().ToString()}/{status.GetMaxHealth().ToString()}"
            + $"\nMP: {status.GetMana().ToString()}/{status.GetMaxMana().ToString()}",
            solidBoxStyle);

        if (!isInBattle)
        {
            GUI.Box(new Rect(Screen.width / 2 - 200, Screen.height - 30, 400, 30),
                $"Lv: {status.GetLevel().ToString()}          XP: {status.GetExp().ToString()}/{status.GetMaxExp().ToString()}");
        }
        else
        {
            // enemy stats
            GUI.Box(new Rect(0, 0, 100, 30),
                $"HP: {currentEnemyStatus.GetHealth().ToString()}/{currentEnemyStatus.GetMaxHealth().ToString()}",
                solidBoxStyle);

            // player turn?
            if (playerTurn)
            {
                int totalSpells = status.spellBook.Count;
                for (int i = 0; i < totalSpells; ++i)
                {
                    Spell s = status.spellBook[i];
                    if (GUI.Button(new Rect(Screen.width / 2 - 160 + (i * 110), Screen.height / 2 - 50, 100, 100),
                        s.Name + (s.Friendly ? "\nHEAL:" : "\nDMG: ") + s.Damage + "\n" + s.Cost + "MP"))
                    {
                        SelectSpell(s); // Call function to handle selection
                    }
                }
                // display spell list, select spell
                // once selected, playerturn = false
                // set executing = true
                // start coroutine to apply player spell & get enemy spell to use
                // animate player spell, yield wait, animate enemy spell, yield wait
                // set executing = false, playerturn = true
            }
        }
    }

    void SelectSpell(Spell selectedSpell)
    {
        Debug.Log($"Selected spell: {selectedSpell.Name}");

        if (status.GetMana() >= selectedSpell.Cost)
        {
            status.UseMana(selectedSpell.Cost);
            playerTurn = false; // End player turn
            StartCoroutine(ExecuteTurn(selectedSpell));
        }
        else
        {
            Debug.Log("Not enough mana!");
        }
    }

    IEnumerator ExecuteTurn(Spell spell)
    {
        executing = true;

        Debug.Log($"Casting {spell.Name}!");

        // animations
        animator.SetTrigger("IsCasting");

        yield return new WaitForSeconds(2f); // casting time

        if (spell.Friendly) // heal
        {
            status.AddHealth(spell.Damage);
        }
        else
        {
            currentEnemyController.TakeDamage(spell.Damage);
        }

        yield return new WaitForSeconds(2f);

        if (currentEnemyStatus.isAlive())
        {
            Debug.Log("enemy turn");
            currentEnemyController.useSpell();
            yield return new WaitForSeconds(1f);
            if (status.isAlive())
            {
                animator.SetTrigger("IsHit");

                yield return new WaitForSeconds(2f);
                playerTurn = true;
            }
        }

        executing = false;
    }

    IEnumerator StartCombat()
    {
        isInBattle = true;
        isControllable = false;
        mouse.enabled = false;
        cam.enabled = true;
        cam.SwapCamera(IsInBattle);

        //animation
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 1);
        animator.SetBool("IsMovingHorizontal", false);
        animator.SetBool("IsMovingVertical", true);

        yield return new WaitForSeconds(1f);
        cam.enabled = false;
        yield return new WaitForSeconds(1f);
        moveScript.enabled = false;

        //reset anim
        animator.SetFloat("Vertical", 0);
        animator.SetBool("IsMovingVertical", false);
        animator.SetBool("IsInBattle", true);

        playerTurn = true;
        Cursor.lockState = CursorLockMode.None;  // Unlocks the cursor
        Cursor.visible = true;                   // Makes the cursor visible
    }

    IEnumerator EndCombat()
    {
        isInBattle = false;
        animator.SetBool("IsInBattle", false);

        status.addExp(20);

        cam.enabled = true;
        cam.SwapCamera(IsInBattle);
        yield return new WaitForSeconds(1f);
        isControllable = true;
        mouse.enabled = true;
        cam.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EndScreen()
    {
        // fade to black, display death screen
        gameOver = true;
        animator.SetBool("IsDead", true);
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / 2;
            yield return null;
        }
        alpha = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInBattle)
        {
            if (!currentEnemy.activeSelf) // enemy dead
            {
                Debug.Log("enemy dead");
                SetActiveChildren(currentArena, false);
                StartCoroutine(EndCombat());
            }
            // turn based combat logic?

            // animation

        }

        GameObject optionalEnemy = CheckForEnemy();
        if (optionalEnemy != null)
        {
            // found enemy!
            currentEnemy = optionalEnemy;
            currentEnemyController = currentEnemy.GetComponent<EnemyController>();
            currentEnemyStatus = currentEnemy.GetComponent<EnemyStatus>();

            currentArena = FindClosestArena();
            SetActiveChildren(currentArena, true);
            Vector3 playerMagicCircle = currentArena.transform.GetChild(2).position;
            Vector3 enemyMagicCircle = currentArena.transform.GetChild(1).position;

            currentEnemyController.StartBattle(playerMagicCircle, enemyMagicCircle);

            moveScript.enabled = true;
            moveScript.SetTarget(playerMagicCircle, enemyMagicCircle);
            StartCoroutine(StartCombat());
        }

        if (Input.GetKeyDown("e"))
        {
            if (isInBattle)
            {
                SetActiveChildren(currentArena, false);
                StartCoroutine(EndCombat());
            }
        }

        if (isControllable)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            // right & up is +
            Vector3 move = transform.right * x + transform.forward * z;

            playerVelocity = move * moveSpeed;

            if (controller.isGrounded && Input.GetButtonDown("Jump"))
            {
                yVelocity = Mathf.Sqrt(jumpHeight * -2f * (gravity));
            }

            //animations
            animator.SetFloat("Horizontal", x);
            animator.SetFloat("Vertical", z);
            animator.SetBool("IsMovingHorizontal", x != 0);
            animator.SetBool("IsMovingVertical", z != 0);

            //Apply  gravity
            yVelocity += gravity * Time.deltaTime;

            playerVelocity.y = yVelocity;

            CollisionFlags flags = controller.Move(playerVelocity * Time.deltaTime);
        }
    }
}
