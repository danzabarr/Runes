using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggeredAbility
{
	public abstract bool Trigger(GameEvent e);
	public abstract void Apply();
}
