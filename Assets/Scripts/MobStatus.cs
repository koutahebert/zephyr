using UnityEngine;
using System.Collections;

public class MobStatus : EnemyStatus
{
    // Use this for initialization
    void Start()
	{
		health = 50f;
		maxHealth = 50f;

		spellBook.Add(new Spell("Icicle", 10, 0, false));
	}
}

