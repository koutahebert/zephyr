using System;
public class Spell
{
	private string name;
	private float damage;
	private float cost;
	private bool friendly; // false for attacks, true for heals

	public Spell(string name, float damage, float cost, bool friendly)
	{
		this.name = name;
		this.damage = damage;
		this.cost = cost;
		this.friendly = friendly;
	}

	public string Name
	{
		get { return name; }
	}

	public float Damage
	{
		get { return damage; }
	}

	public float Cost
	{
		get { return cost; }
	}

	public bool Friendly
	{
		get { return friendly; }
	}
}

