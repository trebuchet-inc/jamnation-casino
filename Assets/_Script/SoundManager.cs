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
		AkSoundEngine.PostEvent("Stop_BattleMusic", gameObject);
	}

	uint _play_Ambiance;
	public void PlayAmbiance () {
		print("lol");
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
		AkSoundEngine.PostEvent("Play_Jingle_Start", gameObject);
	}

	public void CasualCrowd()
	{
		AkSoundEngine.PostEvent("Play_BattleMusic", gameObject);
		AkSoundEngine.PostEvent("Play_Crowd_Extatic", gameObject);
	}

	public void DeceptionCrowd()
	{
		AkSoundEngine.PostEvent("Play_Crowd_Deception", gameObject);
		AkSoundEngine.PostEvent("Stop_BattleMusic", gameObject);
	}

	public void WinJingle()
	{
		AkSoundEngine.PostEvent("Play_Jingle_Success", gameObject);
	}

	public void LoseJingle()
	{
		AkSoundEngine.PostEvent("Play_Jingle_AllMiss", gameObject);
	}

	public void WeaponSelected()
	{
		AkSoundEngine.PostEvent("Play_Weapon_Selected", gameObject);
	}
}
