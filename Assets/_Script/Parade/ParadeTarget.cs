using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParadeTarget : MonoBehaviour 
{
	private void OnTriggerEnter(Collider other)
	{
		// CHECK IF PLAYER IS HERE
		if (!GameRefereeManager.Instance.paradePhase.isReady)
		{
			GameRefereeManager.Instance.paradePhase.SetReady();
		}
	}
}
