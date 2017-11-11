using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	public GameObject currentWeapon;

	private void Start()
	{
		GameRefereeManager.Instance.OnPhaseChanged += OnPhaseChangedHandler;
		GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen += OnWeaponChosenHandler;
	}

	private void OnDisable()
	{
		GameRefereeManager.Instance.OnPhaseChanged += OnPhaseChangedHandler;
		GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen += OnWeaponChosenHandler;
	}

	private void OnPhaseChangedHandler(Phases phases)
	{
		switch (phases)
		{
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

	private void OnWeaponChosenHandler(string s)
	{
		currentWeapon = GameRefereeManager.Instance.weaponSelectionPhase.GetWeaponFromName(s);
		
		SetupWeapon();
	}

	private void SetupWeapon()
	{
		GameObject weapon = Instantiate(currentWeapon, transform.position, Quaternion.identity, transform);
	}

	public void RemoveWeapon()
	{
		Destroy(currentWeapon);
	}
}
