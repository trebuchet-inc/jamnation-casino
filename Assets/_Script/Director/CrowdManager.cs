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
