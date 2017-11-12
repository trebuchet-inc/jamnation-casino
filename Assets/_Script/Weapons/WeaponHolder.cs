using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

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
		GameRefereeManager.Instance.OnPhaseChanged -= OnPhaseChangedHandler;
		GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen  -= OnWeaponChosenHandler;
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
		NVRHand hand = NVRPlayer.Instance.RightHand;
		Weapon _weapon = Instantiate(currentWeapon, NVRPlayer.Instance.transform.position, NVRPlayer.Instance.transform.rotation).GetComponent<Weapon>();
		_weapon.Initialize(hand);
	}

	public void RemoveWeapon()
	{
		Destroy(currentWeapon);
	}
}
