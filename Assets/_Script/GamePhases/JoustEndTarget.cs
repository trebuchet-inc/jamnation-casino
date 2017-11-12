using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustEndTarget: MonoBehaviour
{
	public bool hasHit = false;
	
	private void Start()
	{
		GameRefereeManager.Instance.joustPhase.OnJoustHit += OnJoustHitHandler;
		GameRefereeManager.Instance.OnPhaseChanged += OnPhaseChangedHandler;
	}

	private void OnPhaseChangedHandler(Phases phases)
	{
		if (phases == Phases.Intermission) hasHit = false;
	}

	private void OnJoustHitHandler(Hitinfo hitinfo)
	{
		hasHit = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Entered joustEnd target");

			other.attachedRigidbody.GetComponent<MountAgent>()._freeze = true;
			
			if (!hasHit)
			{
				GameRefereeManager.Instance.joustPhase.EndJoust(false);
			}
		}
	}
}
