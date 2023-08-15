using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
	public void RegisterStartTurnEvents()
	{
		FindObjectOfType<GameManager>().StartTurn += OnStartTurn;
	}

	public void OnStartTurn(object sender, EventArgs eventArgs)
	{

	}

	public void OnTakeDamage(object sender, EventArgs eventArgs)
	{

	}

	
}


