using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : ScriptableObject
{
	public string tooltip;
	public int manaCost;

	public virtual void Cast(Player caster, Player opponent)
	{
		Debug.Log($"{ name } activated");
	}
}
