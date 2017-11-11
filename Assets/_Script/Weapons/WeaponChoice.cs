using NewtonVR;
using UnityEngine;

public class WeaponChoice : MonoBehaviour
{
	public GameObject weaponPresented;

	private NVRHand hand;

	public void SetWeaponChoice()
	{
		print("spawnWeapon");
		Instantiate(weaponPresented, transform.position, Quaternion.identity);
	}

	private void OnTriggerEnter(Collider other)
	{
		// Check if grab
		hand = other.attachedRigidbody.GetComponent<NVRHand>();
		Debug.Log("hand detected");
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
		Debug.Log("hand exit");
		if((object) hand != null) hand = null;
	}
}
