﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndPhase : GamePhase
{
	public int endDuration;
	
	public override void StartPhase()
	{
		StartCoroutine(RestartTimer());
	} 
    
	public override void TerminatePhase()
	{
		if(GameRefereeManager.Instance.isFirstGame) GameRefereeManager.Instance.isFirstGame = false;
	}
	
	public IEnumerator RestartTimer()
	{
		yield return new WaitForSeconds(endDuration);
		
		GameRefereeManager.Instance.NewGame();
	}
}
