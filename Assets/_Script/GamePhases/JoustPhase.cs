using System;
using System.Collections;
using UnityEngine;

public enum Hitinfo
{
	head,
	neck,
	torso
}

public class JoustPhase : GamePhase
{
	public event Action OnJoustGO;
	
	public event Action<Hitinfo	> OnJoustHit;
	
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

	public void callHit(string objname)
	{

	}

	[PunRPC]
	public void ReceiveRegisterHit(int hit)
	{
		if(OnJoustHit != null) OnJoustHit.Invoke((Hitinfo)hit);
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);
	}

	private IEnumerator WaitForGo()
	{
		yield return new WaitForSeconds(3);
		
		if(OnJoustGO != null) OnJoustGO.Invoke();
	}
}
