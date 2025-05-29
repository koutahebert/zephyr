using UnityEngine;
using System.Collections;

public class BossStatus : EnemyStatus
{
	// Use this for initialization
	void Start()
	{
        health = 100f;
        maxHealth = 100f;

        // first 2 spells twice as likely as last 2
        spellBook.Add(new Spell("Icicle", 10, 0, false));
        spellBook.Add(new Spell("Icicle", 10, 0, false));
        spellBook.Add(new Spell("Fire", 25, 0, false));
        spellBook.Add(new Spell("Fire", 25, 0, false));
        spellBook.Add(new Spell("Cure", 10, 0, true));
        spellBook.Add(new Spell("Gravity", 40, 0, false));
    }
}
