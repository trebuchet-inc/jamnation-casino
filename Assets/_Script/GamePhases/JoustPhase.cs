using System;
using System.Collections;
using UnityEngine;

public enum Hitinfo
{
	none = 0,
	head = 1,
	torso = 2,
	leg = 3
}

public class JoustPhase : GamePhase
{
	public event Action OnJoustGO;
	
	public event Action<Hitinfo	> OnJoustHit;

	bool localHited;
	
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
		if(localHited) return;

		localHited = true;
		Hitinfo info = Hitinfo.none;

		if(objname.Contains("root") || objname.Contains("torso"))
		{
			info = Hitinfo.torso;
		}
		else if(objname.Contains("knee") || objname.Contains("foot"))
		{
			info = Hitinfo.leg;
		}
		else if(objname.Contains("neck"))
		{
			info = Hitinfo.head;
		}
		
		if(info == Hitinfo.none)
		{
			print("noneHit");
			return;
		}

		print("hitSend");

		photonView.RPC("ReceiveRegisterHit", PhotonTargets.Others, (int)info);
		if(OnJoustHit != null) OnJoustHit.Invoke(info);
	}

	[PunRPC]
	public void ReceiveRegisterHit(int hit)
	{
		print("hitReceived");
		if(OnJoustHit != null) OnJoustHit.Invoke((Hitinfo)hit);
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);
	}

	private IEnumerator WaitForGo()
	{
		yield return new WaitForSeconds(2.5f);
		
		if(OnJoustGO != null) OnJoustGO.Invoke();
	}
}
