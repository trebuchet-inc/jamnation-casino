using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NewtonVR;

public class WeaponSelectionPhase : GamePhase
{
	public GameObject[] weaponsPool;
	public Transform[] weaponChoiceAnchorsP1;
	public Transform[] weaponChoiceAnchorsP2;

	public event Action<string> OnWeaponChosen;
	
	public bool isWeaponChosen = false;
	public bool isEnemyWeaponChosen = false;
	
	protected override void OnNewGameHandler()
	{
		ResetWeaponsSelection();
	}

	public override void StartPhase()
	{
		isWeaponChosen = false;
		isEnemyWeaponChosen = false;

		NetworkPlayerManager.Instance.SetLocalPlayer();
	}

	public override void TerminatePhase()
	{
		
	}

	public void ChooseWeapon(string weaponName)
	{
		Debug.Log("Chose " + weaponName);

		isWeaponChosen = true;
		
		if(OnWeaponChosen != null) OnWeaponChosen.Invoke(weaponName);
		
		photonView.RPC("ReceiveWeaponChosen", PhotonTargets.Others, weaponName, NetworkPlayerManager.Instance.personalID);
		
		if (CheckIfPhaseComplete())
		{
			GameRefereeManager.Instance.ChangePhase(Phases.Parade);
		}
	}

	private void SetEnemyWeapon(string weaponName, int id)
	{
		GameObject enemyCurrentWeapon = null;

		foreach (GameObject weapon in weaponsPool)
		{
			if (weapon.name == weaponName)
			{
				enemyCurrentWeapon = weapon;
			}
		}

		NVRHand hand = (NVRHand)NetworkPlayerManager.Instance.GetNetworkPlayerHand(id, Handedness.Right);
		Weapon _weapon = Instantiate(enemyCurrentWeapon, hand.transform.parent.position, hand.transform.parent.rotation).GetComponent<Weapon>();
		_weapon.Initialize(hand);
	}
	
	protected override bool CheckIfPhaseComplete()
	{
		return isWeaponChosen && isEnemyWeaponChosen;
	}
	
	private void ResetWeaponsSelection()
	{
		Transform[] weaponChoiceAnchors = NetworkPlayerManager.Instance.personalID == 0 ? weaponChoiceAnchorsP1 : weaponChoiceAnchorsP2;
		
		for(int i = 0; i < weaponChoiceAnchors.Length; i++)
		{
			Instantiate(weaponsPool[i], weaponChoiceAnchors[i].position,  weaponChoiceAnchors[i].rotation);
		}
	}

	[PunRPC]
	public void ReceiveWeaponChosen(string weaponName, int id)
	{
		isEnemyWeaponChosen = true;
		SetEnemyWeapon(weaponName, id);

		if (CheckIfPhaseComplete())
		{
			GameRefereeManager.Instance.ChangePhase(Phases.Parade);
		}
	}
}
