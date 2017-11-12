using System;
using System.Collections;
using UnityEngine;

public class JoustPhase : GamePhase
{
	public event Action OnJoustGO;
	
	public event Action OnJoustHit;
	
	public override void StartPhase()
	{
		StartCoroutine(WaitForGo());
	} 
    
	public override void TerminatePhase()
	{
        
	}

	public void RegisterHit()
	{
		photonView.RPC("ReceiveRegisterHit", PhotonTargets.All);
	}

	[PunRPC]
	public void ReceiveRegisterHit()
	{
		if(OnJoustHit != null) OnJoustHit.Invoke();
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);
	}

	private IEnumerator WaitForGo()
	{
		yield return new WaitForSeconds(2.5f);
		
		if(OnJoustGO != null) OnJoustGO.Invoke();
	}
}
