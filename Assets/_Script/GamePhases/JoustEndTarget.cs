﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoustEndTarget: MonoBehaviour
{
	public bool hasHit = false;
	
	private void Start()
	{
		GameRefereeManager.Instance.joustPhase.OnJoustHit += OnJoustHitHandler;
	}

	private void OnJoustHitHandler(Hitinfo hitinfo)
	{
		hasHit = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if(!hasHit) GameRefereeManager.Instance.joustPhase.EndJoust();
		}
	}
}
