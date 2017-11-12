using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPhase : GamePhase
{
	public int endDuration;
	
	public override void StartPhase()
	{
		
	} 
    
	public override void TerminatePhase()
	{
        
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
