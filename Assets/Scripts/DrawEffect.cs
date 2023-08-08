using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Draw", menuName = "Effects/Draw")]
public class DrawEffect : Effect
{
	public int drawAmount;

	public override void Cast(Player caster, Player opponent)
	{
		for (int i = 0; i < drawAmount; i++)
		{
			caster.Draw(drawAmount);
			new WaitForSeconds(0.5f);
		}
	}
}
