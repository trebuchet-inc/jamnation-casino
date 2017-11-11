using System;
using UnityEngine;

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

	private GamePhase currentPhaseScript;
	public WeaponSelectionPhase weaponSelectionPhase;
	public ParadePhase paradePhase;
	
	public Phases currentPhase;

	public event Action OnNewGame;
	
	public event Action<Phases> OnPhaseChanged; 
	

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		GetReferences();
	}

	private void GetReferences()
	{
		weaponSelectionPhase = GetComponent<WeaponSelectionPhase>();
	}

	public void NewGame()
	{
		if(OnNewGame != null) OnNewGame.Invoke();
		
		photonView.RPC("ReceiveNewGame", PhotonTargets.All);
	}

	[PunRPC]
	public void ReceiveNewGame()
	{
		Debug.Log("Starting New Game");
		ChangePhase(Phases.WeaponSelection);
	}
	
	public void ChangePhase(Phases phase)
	{
		Debug.Log("Changing phase to " + phase.ToString());
		
		// END CURRENT PHASE
		if(currentPhaseScript != null) currentPhaseScript.EndPhase();
		
		// SEND EVENTS TO OTHERS WHO NEED TO DO SHIT
		
		currentPhase = phase;
		if(OnPhaseChanged != null) OnPhaseChanged.Invoke(phase);
		
		switch (phase) // DO GENERAL SHIT
		{
			case Phases.WeaponSelection:
				currentPhaseScript = weaponSelectionPhase;
				weaponSelectionPhase.StartPhase();
			break;
				
			case Phases.Parade:
				currentPhaseScript = paradePhase;
				paradePhase.StartPhase();
			break;
				
			case Phases.Joust:
				
			break;	
				
			case Phases.Intermission:
				
			break;
				
			case Phases.End:
				
			break;
		}
	}
}
