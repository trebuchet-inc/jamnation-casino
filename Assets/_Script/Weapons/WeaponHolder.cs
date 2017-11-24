using UnityEngine;
using NewtonVR;
using System;

public class WeaponHolder : MonoBehaviour
{
	public GameObject currentWeapon;

	public Weapon activeWeapon;

	private void Start()
	{
		GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen += OnWeaponChosenHandler;
		GameRefereeManager.Instance.intermissionPhase.OnReset += OnResetHandler;
	}

	private void OnResetHandler()
	{
		print("RESET");
		RemoveWeapon();
	}

	private void OnDisable()
	{
		//GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen -= OnWeaponChosenHandler;
	}
	
	private void OnWeaponChosenHandler(string s)
	{
		currentWeapon = GameRefereeManager.Instance.weaponSelectionPhase.GetWeaponFromName(s);
		
		SetupWeapon();
	}

	private void SetupWeapon()
	{
		NVRHand hand = NVRPlayer.Instance.RightHand;
		activeWeapon = Instantiate(currentWeapon, NVRPlayer.Instance.transform.position, NVRPlayer.Instance.transform.rotation).GetComponent<Weapon>();
		activeWeapon.Initialize(hand);
	}

	public void RemoveWeapon()
	{
		Weapon[] sada = FindObjectsOfType<Weapon>();
		foreach(Weapon lol in sada)
		{
			Destroy(lol.gameObject);
		}
	}
}
