using System;
using UnityEngine;

public class CrowdManager : FeedbackManager 
{
	public static CrowdManager Instance;
	
	public CharacterAnimatorController[] characters;
	
	void Awake()
	{
		Instance = this;
		characters = FindObjectsOfType<CharacterAnimatorController>();		
	}

	void Start () {
		foreach(CharacterAnimatorController c in characters)
		{
			c.Initialize();
		}		
	}
	
	//
	// Event handlers
	//

	protected override void OnPhaseStartedHandler(Phases phases)
	{
		switch (phases) 
		{
			case Phases.WeaponSelection:
				SetHype(4);
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
	
	protected override void OnJoustHitHandler(LimbType hitinfo)
	{
		SetHype(3);
	}

	//
	// Feedback functions
	//

	public void SetHype(int hype)
	{
		foreach(CharacterAnimatorController c in characters)
		{
			c.SetNextLevel(hype);
		}
	}
}
