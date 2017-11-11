using NewtonVR;
using UnityEngine;

public class WeaponChoice : MonoBehaviour
{
	public GameObject weaponPresented;

	private NVRHand hand;

	public void SetWeaponChoice()
	{
		Instantiate(weaponPresented, transform.position, Quaternion.identity);
	}

	private void OnTriggerEnter(Collider other)
	{
		// Check if grab
		hand = other.GetComponent<NVRHand>();
	}

	private void OnTriggerStay(Collider other)
	{
		if ((object) hand != null && hand.HoldButtonDown)
		{
			GameRefereeManager.Instance.weaponSelectionPhase.ChooseWeapon(weaponPresented);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if((object) hand != null) hand = null;
	}
}
