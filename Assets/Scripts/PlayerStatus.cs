using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{

    public float health = 100.0f;
    private float maxHealth = 100.0f;
    private float mana = 50.0f;
    private float maxMana = 50.0f;

    private int level = 1;
    private int exp = 0;
    private int maxExp = 30;

    public List<Spell> spellBook = new List<Spell>();

    private bool isDead = false;

    private PlayerController playerController;

    public void AddHealth(float moreHealth)
    {
        health += moreHealth;
        if (health > maxHealth)
            health = maxHealth;
    }

    public void addExp(int amount)
    {
        exp += amount;
        if (exp >= maxExp)
        {
            ++level;
            exp -= maxExp;

            Debug.Log("Level up!");
            maxExp += 10;
            maxMana += 10;
            maxHealth += 20;
            mana = maxMana;
            health = maxHealth;
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMana()
    {
        return mana;
    }

    public float GetMaxMana()
    {
        return maxMana;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetExp()
    {
        return exp;
    }

    public int GetMaxExp()
    {
        return maxExp;
    }

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        spellBook.Add(new Spell("Icicle", 12, 3, false));
        spellBook.Add(new Spell("Firebolt", 30, 10, false));
        spellBook.Add(new Spell("Regenerate", 30, 15, true));
    }

    public bool isAlive() { return !isDead; }

    public void UseMana(float mana)
    {
        this.mana -= mana;
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        //Debug.Log("Ouch! " + health);
        if (health <= 0)
        {
            health = 0;
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        isDead = true;
        print("Dead!");
        HideCharacter();
        playerController.EndScreen();
        yield return new WaitForSeconds(5);
    }

    void HideCharacter()
    {
        playerController.IsControllable = false;
    }
}
