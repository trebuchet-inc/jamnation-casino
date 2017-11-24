using UnityEngine;

public class SoundManager : FeedbackManager 
{
	public static SoundManager Instance;

	private void Awake()
	{
		Instance = this;
	}
//
// Event Handlers
//
	
	protected override void OnPhaseStartedHandler(Phases phases)
	{
		switch (phases) 
		{
			case Phases.WeaponSelection:
				
				break;
				
			case Phases.Parade:
				
				break;
				
			case Phases.Joust:
				
				break;
				
			case Phases.Intermission:
				
				break;
				
			case Phases.End:
				
				break;
		}
	}

//
// Feedback Functions
//
	
	public void PlayHit (WeaponType weapon) {
		switch(weapon)
		{
			 case WeaponType.spear :
			 AkSoundEngine.PostEvent("Play_Lance_hit", gameObject);
			 break;

			 case WeaponType.axe :
			 AkSoundEngine.PostEvent("Play_Axe_hit", gameObject);
			 break;

			 case WeaponType.scourge :
			 AkSoundEngine.PostEvent("Play_Flail_hit", gameObject);
			 break;
		}

		AkSoundEngine.PostEvent("Play_Crowd_Surprise", gameObject);
		AkSoundEngine.PostEvent("Stop_BattleMusic", gameObject);
	}

	public void PlayAmbiance () 
	{
		AkSoundEngine.PostEvent("Play_Intro_Annonceurs", gameObject);
		uint _play_Ambiance = AkSoundEngine.GetIDFromString("Play_Ambiance");
		AkSoundEngine.PostEvent(_play_Ambiance, gameObject);
	}

	public void StopAmbiance()
	{
		uint _stop_Ambiance = AkSoundEngine.GetIDFromString("Stop_Ambiance");
		AkSoundEngine.PostEvent(_stop_Ambiance, gameObject);
	}

	public void ResetAmbiance()
	{
		StopAmbiance();
		PlayAmbiance();
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
