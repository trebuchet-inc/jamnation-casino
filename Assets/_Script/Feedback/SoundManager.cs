using UnityEngine;

public class SoundManager : FeedbackManager 
{
	public static SoundManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	protected override void Start()
	{
		base.Start();
		GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen += WeaponSelected;
		GameRefereeManager.Instance.joustPhase.OnJoustHit += PlayHit;
	}

	//
	// Event Handlers
	//
	
	protected override void OnPhaseStartedHandler(Phases phases)
	{
		switch (phases) 
		{
			case Phases.WeaponSelection:
				PlayLogoJingle();
				break;
				
			case Phases.Parade:
				StartJingle();
				break;
				
			case Phases.Joust:
				PlayBattleMusic();
				break;
				
			case Phases.Intermission:
				StopBattleMusic();
				break;
				
			case Phases.End:
				break;
		}
	}

	//
	// Feedback Functions
	//
	
	public void PlayHit (HitInfo info) {
		switch((WeaponType)info.weaponUsed)
		{
			 case WeaponType.Spear :
			 AkSoundEngine.PostEvent("Play_Lance_hit", gameObject);
			 break;

			 case WeaponType.Axe :
			 AkSoundEngine.PostEvent("Play_Axe_hit", gameObject);
			 break;

			 case WeaponType.Flail :
			 AkSoundEngine.PostEvent("Play_Flail_hit", gameObject);
			 break;
		}
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
	
	public void StartJingle()
	{
		AkSoundEngine.PostEvent("Play_Jingle_Start", gameObject);
	}

	public void PlayBattleMusic()
	{
		AkSoundEngine.PostEvent("Play_BattleMusic", gameObject);
	}

	public void StopBattleMusic()
	{
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

	public void WeaponSelected(WeaponType type)
	{
		AkSoundEngine.PostEvent("Play_Weapon_Selected", gameObject);
	}

	public void ChangeCrowdHype(int value)
	{
		AkSoundEngine.SetRTPCValue("Crowd_Hype", value);
	}

	public void PlayLogoJingle()
	{
		AkSoundEngine.PostEvent("Play_Logo_Jingle", gameObject);
	}
}
