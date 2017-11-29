using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndPhase : GamePhase
{
	public int endDuration;

	public event Action OnJoustEnded;
	
	public override void StartPhase()
	{
		StartCoroutine(RestartTimer());
		
		
	} 
    
	public override void TerminatePhase()
	{
		
	}
	
	public IEnumerator RestartTimer()
	{
		yield return new WaitForSeconds(endDuration);
		
		if(GameRefereeManager.Instance.isFirstGame) GameRefereeManager.Instance.isFirstGame = false;
		
		GameRefereeManager.Instance.NewGame();
	}
	
	
}
