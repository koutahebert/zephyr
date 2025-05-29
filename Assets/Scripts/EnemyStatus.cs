using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.XR;

public abstract class EnemyStatus : MonoBehaviour
{
	public float health;
	public float maxHealth;

    public EnemyController controller;

	public List<Spell> spellBook = new List<Spell>();

	private bool isDead = false;

    public void AddHealth(float moreHealth)
    {
        health += moreHealth;
        if (health > maxHealth)
            health = maxHealth;
    }

    public virtual bool isAlive()
	{
		return !isDead;
	}

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public virtual void ApplyDamage(float damage)
	{
        health -= damage;
        Debug.Log(health);
        if (health <= 0 && !isDead)
        {
            health = 0;
			StartCoroutine(Die());
        }
    }
	
	IEnumerator Die()
	{
		isDead = true;
        print("***********Dead!*************");
        yield return null;
    }
}

