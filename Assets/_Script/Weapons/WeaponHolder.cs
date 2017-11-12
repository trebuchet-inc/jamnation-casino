using UnityEngine;
using NewtonVR;

public class WeaponHolder : MonoBehaviour
{
	public GameObject currentWeapon;

	public Weapon activeWeapon;

	private void Start()
	{
		GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen += OnWeaponChosenHandler;
		GameRefereeManager.Instance.OnPhaseChanged += OnPhaseChangedHandler;
	}

	private void OnPhaseChangedHandler(Phases phases)
	{
		if (GameRefereeManager.Instance.currentPhase == Phases.WeaponSelection && GameRefereeManager.Instance.roundIndex >= 1)
		{
			RemoveWeapon();
		}
	}

	private void OnDisable()
	{
		GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen -= OnWeaponChosenHandler;
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
		if(currentWeapon != null) Destroy(activeWeapon.gameObject);
	}
}
