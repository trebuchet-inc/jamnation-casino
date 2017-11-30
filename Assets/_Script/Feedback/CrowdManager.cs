using System;
using UnityEngine;

public class CrowdManager : FeedbackManager 
{
	public static CrowdManager Instance;
	
	public CharacterAnimatorController[] characters;

	public float HypeLevel;
	public float HypeDecreaseRate = 0.1f;
	
	void Awake()
	{
		Instance = this;
		characters = FindObjectsOfType<CharacterAnimatorController>();		
	}

	protected override void Start() 
	{
		base.Start();
		
		foreach(CharacterAnimatorController c in characters)
		{
			c.Initialize();
		}		
	}

	void Update()
	{
		HypeLevel += -100 * Time.deltaTime * Mathf.Sign(HypeLevel) * HypeDecreaseRate;
		SoundManager.Instance.ChangeCrowdHype((int)HypeLevel);
	}
	
	//
	// Event handlers
	//

	protected override void OnPhaseStartedHandler(Phases phases)
	{
		switch (phases) 
		{
			case Phases.WeaponSelection:
				HypeDecreaseRate = 0.1f;
				SetHype(2);
				break;
				
			case Phases.Parade:
				SetHype(4);
				break;
				
			case Phases.Joust:
				
				break;
				
			case Phases.Intermission:
				SetHype(4);
				break;
				
			case Phases.End:
				SetHype(5);
				HypeDecreaseRate = 0;
				break;
		}
	}
	
	protected override void OnJoustHitHandler(HitInfo hitinfo)
	{
		SetHype(3);
	}

	//
	// Feedback functions
	//

	public void SetHype(int hype, int sign = 1)
	{
		foreach(CharacterAnimatorController c in characters)
		{
			HypeLevel = hype * 20 * sign;
			c.SetNextLevel(hype);
		}
	}
}
