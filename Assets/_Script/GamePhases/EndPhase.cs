using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndPhase : GamePhase
{
	public int endDuration;

	public event Action OnReset;
	
	public override void StartPhase()
	{
		StartCoroutine(RestartTimer());
	} 
    
	public override void TerminatePhase()
	{
		if(GameRefereeManager.Instance.isFirstGame) GameRefereeManager.Instance.isFirstGame = false;
		if(OnReset != null) OnReset.Invoke();
	}
	
	public IEnumerator RestartTimer()
	{
		yield return new WaitForSeconds(endDuration);
		
		photonView.RPC("ReceiveRestartGame", PhotonTargets.All);
	}

	[PunRPC]
	public void ReceiveRestartGame()
	{
		GameRefereeManager.Instance.NewGame();
	}
}
