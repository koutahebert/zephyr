using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class EnemyController : MonoBehaviour
{
	public EnemyStatus status;
    public PlayerStatus playerStatus;

    public CharacterController controller;
    public Animation anim;

    public MoveToPosition moveScript;

    private State currentState;
    private bool isInBattle = false;
    private bool isHit = false;
    private bool isCasting = false;
    private bool isMoving = false;

    public virtual Spell chooseSpell()
    {
        return status.spellBook[Random.Range(0, status.spellBook.Count)];
    }

    public virtual void useSpell()
    {
        StartCoroutine(Cast(chooseSpell()));
    }

    IEnumerator Cast(Spell s)
    {
        isCasting = true;
        Debug.Log($"{s.Name}");

        yield return new WaitForSeconds(1f);
        isCasting = false;

        if (s.Friendly) // heal
        {
            status.AddHealth(s.Damage);
        }
        else
        {
            playerStatus.ApplyDamage(s.Damage);
        }

        yield return new WaitForSeconds(1f);
    }

    public abstract void StartBattle(Vector3 playerMagicCircle, Vector3 enemyMagicCircle);

    public State GetState
    {
        get { return currentState; }
    }

    public void BeIdle()
    {
        anim.CrossFade("idle", 0.2f);
        anim["idle"].speed = 1.0f;
    }

    public void BeMoving()
    {
        anim.CrossFade("run", 0.2f);
        anim["run"].speed = 1.0f;
    }

    public void BeBattleIdle()
    {
        anim.CrossFade("waitingforbattle", 0.2f);
        anim["waitingforbattle"].speed = 1.0f;
    }

    public void BeHit()
    {
        anim.CrossFade("gethit", 0.2f);
        anim["gethit"].speed = 1.0f;
    }

    public void BeCasting()
    {
        anim.CrossFade("attack", 0.2f);
        anim["attack"].speed = 1.0f;
    }

    public void BeDead()
    {
        anim.CrossFade("die", 0.2f);
        anim["die"].speed = 1.0f;
        StartCoroutine(timedDeactivation(2));
    }

    IEnumerator timedDeactivation(float seconds)
    {
        //Wait for several seconds
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    public void ChangeState(State newState)
    {
        currentState = newState;
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(HandleDamage(damage));
    }

    IEnumerator HandleDamage(float damage)
    {
        isHit = true;
        yield return new WaitForSeconds(1f);
        status.ApplyDamage(damage);
        isHit = false;
    }

    public bool IsInBattle
    {
        get { return isInBattle; }
        set { isInBattle = value; }
    }

    public bool IsHit
    {
        get { return isHit; }
        set { isHit = value; }
    }

    public bool IsDead
    {
        get { return status.IsDead; }
        set { status.IsDead = value; }
    }

    public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }

    public bool IsCasting
    {
        get { return isCasting; }
        set { isCasting = value; }
    }
}
