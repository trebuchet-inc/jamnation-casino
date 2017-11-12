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
	
	public event Action<Hitinfo> OnJoustHit;

	bool localHited;
	bool _active;

	Hitinfo info = Hitinfo.none;
	
	public override void StartPhase()
	{
		StartCoroutine(WaitForGo());
		_active = true;
		info = Hitinfo.none;

		SoundManager.Instance.CasualCrowd();
	} 
    
	public override void TerminatePhase()
	{
        _active = false;
		HMDisplayUIManager.Instance.ShowResult(info);
	}

	public void EndJoust(bool hit)
	{
		photonView.RPC("ReceiveEndJoust", PhotonTargets.All, hit);
	}

	[PunRPC]
	public void ReceiveEndJoust(bool hit)
	{
		if(!hit) SoundManager.Instance.DeceptionCrowd();
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);
	}

	public void callHit(string objname, string weapon)
	{
		if(localHited || !_active) return;

		localHited = true;

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

		photonView.RPC("ReceiveRegisterHit", PhotonTargets.Others, (int)info, weapon);
		if(OnJoustHit != null) OnJoustHit.Invoke(info);
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);
	}

	[PunRPC]
	public void ReceiveRegisterHit(int hit, string weapon)
	{
		print("hitReceived");
		if(OnJoustHit != null) OnJoustHit.Invoke((Hitinfo)hit);
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);

		SoundManager.Instance.PlayHit(weapon);

		Fade.Instance.StartFade(0.2f,0.1f);
		StartCoroutine(UnFade());
	}

	private IEnumerator UnFade()
	{
		yield return new WaitForSeconds(1.0f);

		Fade.Instance.StartFade(1f,2f);
	}

	private IEnumerator WaitForGo()
	{
		yield return new WaitForSeconds(2.5f);
		
		if(OnJoustGO != null) OnJoustGO.Invoke();
	}
}
