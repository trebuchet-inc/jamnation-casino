using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class JoustPhase : GamePhase
{
	public event Action OnJoustGO;
	
	public event Action<LimbType> OnJoustHit;

	public GameObject blood;

	bool localHited;
	bool _active;

	LimbType lastHit;
	
	public override void StartPhase()
	{
		StartCoroutine(WaitForGo());
		lastHit = LimbType.None;
		_active = true;
		localHited = false;

		SoundManager.Instance.CasualCrowd();
	} 
    
	public override void TerminatePhase()
	{
        _active = false;
		HMDisplayUIManager.Instance.ShowResult(lastHit);
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

	public void callHit(HitInfo info)
	{
		if(localHited || !_active) return;

		localHited = true;
		lastHit = (LimbType)info.limbHited; 
		float multiplier = 1;

		switch(lastHit)
		{
			case LimbType.Head :
			multiplier = 3;
			break;

			case LimbType.Hand :
			multiplier = 1;
			break;
			
			case LimbType.Torso :
			multiplier = 2;
			break;
		}

		print("hitSend to " + lastHit);

		if(lastHit == LimbType.None) return;

		photonView.RPC("ReceiveRegisterHit", PhotonTargets.Others, SerializationToolkit.ObjectToByteArray(info));

		ScoreManager.Instance.AddScoreBlue(multiplier);
		
		if(OnJoustHit != null) OnJoustHit.Invoke((LimbType)info.limbHited);
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);

		Instantiate(blood, info.hitPoint.Deserialize(), Quaternion.identity);
	}

	[PunRPC]
	public void ReceiveRegisterHit(byte[] data)
	{
		print("hitReceived");

		HitInfo info = (HitInfo)SerializationToolkit.ByteArrayToObject(data);

		float multiplier = 1;

		switch((LimbType)info.limbHited)
		{
			case LimbType.Head :
			multiplier = 3;
			break;

			case LimbType.Hand :
			multiplier = 1;
			break;
			
			case LimbType.Torso :
			multiplier = 2;
			break;
		}
		
		ScoreManager.Instance.AddScoreRed(multiplier);
		
		if(OnJoustHit != null) OnJoustHit.Invoke((LimbType)info.limbHited);
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);

		SoundManager.Instance.PlayHit((WeaponType)info.weaponUsed); 

		Fade.Instance.StartFade(0.2f,0.1f);
		StartCoroutine(UnFade());

		Instantiate(blood, info.hitPoint.Deserialize(), Quaternion.identity);
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
