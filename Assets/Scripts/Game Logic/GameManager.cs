using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public delegate bool Trigger(GameEvent evt);

	public event EventHandler StartTurn;
	public event EventHandler EndTurn;
	public event EventHandler<DamageEventArgs> Damage;

	public class DamageEventArgs : EventArgs
	{
		public Unit source, target;
		public Spell spell;
		public int damage;
	}

	public delegate bool Filter<T>(T args) where T : EventArgs;

	public delegate void Invocation(object sender, EventArgs eventArgs);

	public void RegisterDamageEvents(Filter<DamageEventArgs> filter, Action<object, DamageEventArgs> callback)
	{
		Damage += (object sender, DamageEventArgs eventArgs) =>
		{
			if (filter(eventArgs))
				callback.Invoke(sender, eventArgs);
		};
	}


	private void OnStartTurn()
	{
		StartTurn?.Invoke(this, EventArgs.Empty);
	}

	private void OnEndTurn()
	{
		EndTurn?.Invoke(this, EventArgs.Empty);
	}

	private void OnDamage(DamageEventArgs e)
	{
		Damage?.Invoke(this, e);
	}
}
