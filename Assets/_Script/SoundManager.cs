 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour 
{
	public static SoundManager Instance;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
	}
	
	public void PlayHit (string weapon) {
		switch(weapon)
		{
			 case "lance" :
			 AkSoundEngine.PostEvent("Play_Lance_hit", gameObject);
			 break;

			 case "hache" :
			 AkSoundEngine.PostEvent("Play_Axe_hit", gameObject);
			 break;

			 case "fleau" :
			 AkSoundEngine.PostEvent("Play_Flail_hit", gameObject);
			 break;
		}

		AkSoundEngine.PostEvent("Play_Crowd_Surprise", gameObject);
		CrowdManager.Instance.SetHype(3);
		AkSoundEngine.PostEvent("Stop_BattleMusic", gameObject);
	}

	uint _play_Ambiance;
	public void PlayAmbiance () {
		AkSoundEngine.PostEvent("Play_Intro_Annonceurs", gameObject);
		_play_Ambiance = AkSoundEngine.GetIDFromString("Play_Ambiance");
		AkSoundEngine.PostEvent("Play_Ambiance", gameObject);
	}

	uint _stop_Ambiance;
	public void StopAmbiance()
	{
		_stop_Ambiance = AkSoundEngine.GetIDFromString("Stop_Ambiance");
		AkSoundEngine.PostEvent(_stop_Ambiance, gameObject);
	}
	
	public void HypeCrowd()
	{
		AkSoundEngine.PostEvent("Play_Crowd_GetHyped", gameObject);
		CrowdManager.Instance.SetHype(4);
		AkSoundEngine.PostEvent("Play_Jingle_Start", gameObject);
		CrowdManager.Instance.SetHype(2);
	}

	public void CasualCrowd()
	{
		AkSoundEngine.PostEvent("Play_BattleMusic", gameObject);
		AkSoundEngine.PostEvent("Play_Crowd_Extatic", gameObject);
		CrowdManager.Instance.SetHype(5);
	}

	public void DeceptionCrowd()
	{
		AkSoundEngine.PostEvent("Play_Crowd_Deception", gameObject);
		CrowdManager.Instance.SetHype(1);
		AkSoundEngine.PostEvent("Stop_BattleMusic", gameObject);
	}

	public void WinJingle()
	{
		AkSoundEngine.PostEvent("Play_Jingle_Success", gameObject);
		CrowdManager.Instance.SetHype(5);
	}

	public void LoseJingle()
	{
		AkSoundEngine.PostEvent("Play_Jingle_AllMiss", gameObject);
		CrowdManager.Instance.SetHype(1);
	}

	public void WeaponSelected()
	{
		AkSoundEngine.PostEvent("Play_Weapon_Selected", gameObject);
	}
}
