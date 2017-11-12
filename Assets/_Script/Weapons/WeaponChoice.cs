using System;
using NewtonVR;
using UnityEngine;

public class WeaponChoice : MonoBehaviour
{
	public GameObject dummyWeaponPrefab;
	public GameObject realWeaponPrefab;
	
	public GameObject dummy;

	private NVRHand hand;

	public void SetWeaponChoice()
	{
		print("spawnWeapon");
		dummy = Instantiate(dummyWeaponPrefab, transform.position, Quaternion.identity);
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
			GameRefereeManager.Instance.weaponSelectionPhase.ChooseWeapon(realWeaponPrefab.name);
			SoundManager.Instance.WeaponSelected();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if((object) hand != null) hand = null;
	}

	public void Deactivate()
	{
		Destroy(dummy);
	}
}
