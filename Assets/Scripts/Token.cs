using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Token", menuName = "ScriptableObjects/Token", order = 1)]
public class Token : ScriptableObject
{
	public int manaCost;

	public UnityEvent effect;
}
