using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponSelectionPhase : GamePhase
{
	public GameObject[] weaponsPool;
	public List<GameObject> weaponsAvailable;

	public WeaponChoice[] weaponChoiceAnchors;

	public event Action<string> OnWeaponChosen;

	public GameObject currentWeapon;
	public GameObject enemyCurrentWeapon;

	public bool isWeaponChosen;
	public bool isEnemyWeaponChosen;
	
	protected override void OnNewGameHandler()
	{
		ResetWeaponsAvailable();
	}

	public override void StartPhase()
	{
		PresentWeaponChoice();
	}

	public override void EndPhase()
	{
		
	}
	
	//
	// WEAPON SELECTION
	//

	private void PresentWeaponChoice()
	{
		for (int i = 0; i < weaponsAvailable.Count; i++)
		{
			weaponChoiceAnchors[i].weaponPresented = weaponsAvailable[i];
			weaponChoiceAnchors[i].SetWeaponChoice();
		}
	}

	private void ResetWeaponsAvailable()
	{
		weaponsAvailable.Clear();
		weaponsAvailable = weaponsPool.ToList();
	}

	private void RemoveAvailableWeapon(string weaponName)
	{
		foreach (var w in GetWeaponsAvailable())
		{
			if (w.name == weaponName)
			{
				weaponsAvailable.Remove(w);
			}
		}
	}

	private List<GameObject> GetWeaponsAvailable()
	{
		List<GameObject> copy = new List<GameObject>(weaponsAvailable.Count);

		foreach (var weapon in weaponsAvailable)
		{
			copy.Add(weapon);
		}

		return copy;
	}

	public void ChooseWeapon(GameObject weapon)
	{
		RemoveAvailableWeapon(weapon.name);
		
		Debug.Log("Chose " + weapon.name);

		currentWeapon = weapon;
		isWeaponChosen = true;
		
		if(OnWeaponChosen != null) OnWeaponChosen.Invoke(weapon.name);
		
		photonView.RPC("ReceiveWeaponChosen", PhotonTargets.Others, weapon.name);
		
		if (CheckIfPhaseComplete())
		{
			GameRefereeManager.Instance.ChangePhase(Phases.Parade);
		}
	}

	[PunRPC]
	public void ReceiveWeaponChosen(string weaponName)
	{
		isEnemyWeaponChosen = true;
		SetEnemyWeapon(weaponName);

		if (CheckIfPhaseComplete())
		{
			GameRefereeManager.Instance.ChangePhase(Phases.Parade);
		}
	}

	private void SetEnemyWeapon(string weaponName)
	{
		foreach (var weapon in weaponsPool)
		{
			if (weapon.name == weaponName)
			{
				enemyCurrentWeapon = weapon;
			}
		}
	}
	
	protected override bool CheckIfPhaseComplete()
	{
		return isWeaponChosen && isEnemyWeaponChosen;
	}
}
