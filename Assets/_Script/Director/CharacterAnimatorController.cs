﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour {

	private Animator chanim;
	private int hypeLevel;
	private int nextHypeLevel;
	private float delay;
	private bool hypeChange;


	// Use this for initialization
	public void Initialize () {
		RollModelAndGetAnimator();
		hypeLevel = Random.Range(1,5);
		delay = 5f;
		chanim.SetTrigger(hypeLevel.ToString());
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (hypeChange)
		{
			if (delay <= 0)
			{
				hypeLevel = nextHypeLevel;
				SetNextLevel(hypeLevel);
				hypeChange = false;
			}
		}
		else if (delay <= 0 && hypeLevel > 1)
		{
			hypeLevel --;
			chanim.SetTrigger(hypeLevel.ToString());
			delay = 5f;
		}

		delay =- Time.deltaTime;

		
	}

	public void SetNextLevel (int whatIsTheHype)
	{
		delay = Random.Range(0,2.0f);
		hypeChange = true;
		nextHypeLevel = whatIsTheHype;
	}

	private void SetHypeLevel(int whatIsTheHype)
	{
		delay = 5f;
		hypeLevel = whatIsTheHype + Random.Range(-1,2);
		hypeLevel = Mathf.Clamp(hypeLevel,1,5);
		chanim.SetTrigger(hypeLevel.ToString());
	}



	void RollModelAndGetAnimator()
	{
		int who = Random.Range(0,3);
		for (int i = 0; i < 3; i++)
		{
			if (i != who)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}		
			else 
			{
				chanim = transform.GetChild(i).GetComponentInChildren<Animator>();
			}	
		}

	}
}
