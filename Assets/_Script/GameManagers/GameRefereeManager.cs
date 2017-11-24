using System;
using NewtonVR;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum Phases
{
	WeaponSelection		= 0,
	Parade				= 1,
	Joust				= 2,
	Intermission		= 3,
	End					= 4
}

public class GameRefereeManager : Photon.MonoBehaviour
{
	public static GameRefereeManager Instance;

	public bool isFirstGame = true;
	public int roundIndex;
	public int TotalRounds;
	
	// GAME PHASES //
	public Phases currentPhase;
	private GamePhase currentPhaseScript;
	
	public WeaponSelectionPhase weaponSelectionPhase;
	public ParadePhase paradePhase;
	public JoustPhase joustPhase;
	public IntermissionPhase intermissionPhase;
	public EndPhase endPhaseScript;

	public event Action OnNewGame;
	
	public event Action<Phases> OnPhaseStarted; 
	public event Action<Phases> OnPhaseEnded; 
	

	private void Awake()
	{
		Instance = this;
		GetReferences();
	}

	private void GetReferences()
	{
		weaponSelectionPhase = GetComponent<WeaponSelectionPhase>();
		paradePhase = GetComponent<ParadePhase>();
		joustPhase = GetComponent<JoustPhase>();
		intermissionPhase = GetComponent<IntermissionPhase>();
		endPhaseScript = GetComponent<EndPhase>();
	}

	public void NewGame()
	{
		photonView.RPC("ReceiveNewGame", PhotonTargets.All);
	}


	public void ChangePhase(Phases phase)
	{
		Debug.Log("Changing phase to " + phase.ToString());

		if(phase == currentPhase && roundIndex > 0) return;
		
		// END CURRENT PHASE
		if((object)currentPhaseScript != null) currentPhaseScript.TerminatePhase();
		if(OnPhaseEnded != null && currentPhase != null) OnPhaseEnded.Invoke(currentPhase);
		
		// SEND EVENTS TO OTHERS WHO NEED TO DO SHIT
		
		currentPhase = phase;

		if(OnPhaseStarted != null) OnPhaseStarted.Invoke(currentPhase);

		switch (currentPhase) 
		{
			case Phases.WeaponSelection:
				currentPhaseScript = weaponSelectionPhase;
			break;
				
			case Phases.Parade:
				currentPhaseScript = paradePhase;
			break;
				
			case Phases.Joust:
				currentPhaseScript = joustPhase;
			break;	
				
			case Phases.Intermission:
				currentPhaseScript = intermissionPhase;
			break;
				
			case Phases.End:
				currentPhaseScript = endPhaseScript;
			break;
		}
		
		if((object)currentPhaseScript != null) currentPhaseScript.StartPhase();
	}
	
	//
	// RPC FUNCTIONS
	//
	
	[PunRPC]
	public void ReceiveNewGame()
	{
		Debug.Log("Starting New Game");
		
		NetworkPlayerManager.Instance.SetLocalPlayer();
		roundIndex = 0;
		ScoreBoardManager.Instance.ResetScore();
		
		if(OnNewGame != null) OnNewGame.Invoke();
		ChangePhase(Phases.WeaponSelection);

		SoundManager.Instance.ResetAmbiance();
	}
}
