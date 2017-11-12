using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IntermissionPhase : GamePhase
{
	public int intermissionDuration;

	public event Action OnReset;
	
	public override void StartPhase()
	{
		StartCoroutine(IntermissionTimer());
	} 
    
	public override void TerminatePhase()
	{
        if(OnReset != null) OnReset.Invoke();
	}

	public IEnumerator IntermissionTimer()
	{
		yield return new WaitForSeconds(intermissionDuration);
		
		photonView.RPC("ReceiveEndIntermission", PhotonTargets.All);
	}

	[PunRPC]
	public void ReceiveEndIntermission()
	{
		GameRefereeManager.Instance.ChangePhase(CheckIfMatchEnded());
	}

	public Phases CheckIfMatchEnded()
	{
		return GameRefereeManager.Instance.roundIndex >= GameRefereeManager.Instance.TotalRounds
			? Phases.End
			: Phases.WeaponSelection;
	}
}
