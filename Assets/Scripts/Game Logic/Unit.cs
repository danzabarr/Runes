using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Unit : MonoBehaviour
{
	public event EventHandler SummonEventListeners;
	public event EventHandler DamageEventListeners;
	public event EventHandler DeathEventListeners;
	public event EventHandler AttackEventListeners;

	public delegate void SummonEventHandler(object sender, SummonEventArgs e);
	public delegate void DamageEventHandler(object sender, DamageEventArgs e);
	public delegate void DeathEventHandler(object sender, DeathEventArgs e);
	public delegate void AttackEventHandler(object sender, AttackEventArgs e);

	public class SummonEventArgs : EventArgs
	{
		public Unit Unit { get; set; }
	}

	public class DamageEventArgs : EventArgs
	{
		public Spell Spell { get; set; }
		public int Damage { get; set; }
	}

	public class DeathEventArgs : EventArgs
	{
		public Unit Unit { get; set; }
	}

	public class AttackEventArgs : EventArgs
	{
		public Unit Unit { get; set; }
	}

	private void OnSummon(SummonEventArgs e)
	{
		SummonEventListeners?.Invoke(this, e);
	}

	private void OnDamage(DamageEventArgs e)
	{
		DamageEventListeners?.Invoke(this, e);
	}

	private void OnDeath(DeathEventArgs e)
	{
		DeathEventListeners?.Invoke(this, e);
	}

	private void OnAttack(AttackEventArgs e)
	{
		AttackEventListeners?.Invoke(this, e);
	}
}
