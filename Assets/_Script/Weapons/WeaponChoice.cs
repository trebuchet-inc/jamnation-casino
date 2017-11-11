﻿using System;
using NewtonVR;
using UnityEngine;

public class WeaponChoice : MonoBehaviour
{
	public GameObject weaponPresented;

	public GameObject dummy;

	private NVRHand hand;

	public void SetWeaponChoice()
	{
		print("spawnWeapon");
		dummy = Instantiate(weaponPresented, transform.position, Quaternion.identity);
	}

	private void OnTriggerEnter(Collider other)
	{
		// Check if grab
		hand = other.attachedRigidbody.GetComponent<NVRHand>();
	}

	private void OnTriggerStay(Collider other)
	{
		if ((object) hand != null && !GameRefereeManager.Instance.weaponSelectionPhase.isWeaponChosen && hand.HoldButtonDown)
		{
			GameRefereeManager.Instance.weaponSelectionPhase.ChooseWeapon(weaponPresented.name);
			dummy.SetActive(false);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if((object) hand != null) hand = null;
	}

	public void Deactivate()
	{
		gameObject.SetActive(false);
	}
}
