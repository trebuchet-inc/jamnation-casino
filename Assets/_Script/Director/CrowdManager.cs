using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CrowdManager : MonoBehaviour {

	public CharacterAnimatorController[] characters;
	public static CrowdManager Instance;

	
	void Awake()
	{
		Instance = this;
		characters = FindObjectsOfType<CharacterAnimatorController>();		
	}

	// Use this for initialization
	void Start () {
		foreach(CharacterAnimatorController c in characters)
		{
			c.Initialize();
		}		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetHype(int hype)
	{
		foreach(CharacterAnimatorController c in characters)
		{
			c.SetNextLevel(hype);
		}
	}


}
