﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

	public GameObject blood;

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

	public void callHit(string objname, string weapon, Vector3 pos)
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

		photonView.RPC("ReceiveRegisterHit", PhotonTargets.Others, (int)info, weapon, new SerializableVector3(pos));
		ScoreBoardManager.Instance.DisplayUpdateOnScreen(ScoreBoardManager.Instance.displayLeft, "HIT!");
		ScoreBoardManager.Instance.AddScoreBlue();
		if(OnJoustHit != null) OnJoustHit.Invoke(info);
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);
		Instantiate(blood, pos, Quaternion.identity);
	}

	[PunRPC]
	public void ReceiveRegisterHit(int hit, string weapon, SerializableVector3 pos)
	{
		print("hitReceived");
		
		ScoreBoardManager.Instance.DisplayUpdateOnScreen(ScoreBoardManager.Instance.displayRight, "HIT!");
		ScoreBoardManager.Instance.AddScoreRed();
		
		if(OnJoustHit != null) OnJoustHit.Invoke((Hitinfo)hit);
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);

		SoundManager.Instance.PlayHit(weapon);

		Fade.Instance.StartFade(0.2f,0.1f);
		StartCoroutine(UnFade());
		Instantiate(blood, pos.Deserialize(), Quaternion.identity);
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
