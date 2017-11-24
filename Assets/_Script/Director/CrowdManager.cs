using UnityEngine;

public class CrowdManager : MonoBehaviour {

	public CharacterAnimatorController[] characters;
	public static CrowdManager Instance;
	
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

	public void SetHype(int hype)
	{
		foreach(CharacterAnimatorController c in characters)
		{
			c.SetNextLevel(hype);
		}
	}
}
