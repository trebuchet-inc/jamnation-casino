using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IntermissionPhase : GamePhase
{
	public int intermissionDuration;

	public event Action OnRoundReset;
	
	public override void StartPhase()
	{
		GameRefereeManager.Instance.roundIndex++;

		StartCoroutine(IntermissionTimer());
	} 
    
	public override void TerminatePhase()
	{
        if(OnRoundReset != null) OnRoundReset.Invoke();
	}

	private IEnumerator IntermissionTimer()
	{
		yield return new WaitForSeconds(intermissionDuration);
		
		GameRefereeManager.Instance.ChangePhase(CheckIfMatchEnded());
	}

	private Phases CheckIfMatchEnded()
	{
		return GameRefereeManager.Instance.roundIndex >= GameRefereeManager.Instance.TotalRounds
			? Phases.End
			: Phases.WeaponSelection;
	}
}
