using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponSelectionPhase : GamePhase
{
	public GameObject[] fakeWeaponsPool;
	public GameObject[] weaponsPool;
	public List<GameObject> fakeWeaponsAvailable;
	public List<GameObject> weaponsAvailable;

	public WeaponChoice[] weaponChoiceAnchors;
	public WeaponChoice[] otherWeaponChoiceAnchors;

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
		foreach (var weaponChoice in weaponChoiceAnchors)
		{
			weaponChoice.Deactivate();
		}
	}
	
	//
	// WEAPON SELECTION
	//

	private void PresentWeaponChoice()
	{
		WeaponChoice[] anchors =
			NetworkPlayerManager.Instance.personalID == 0 ? weaponChoiceAnchors : otherWeaponChoiceAnchors;
		
		for (int i = 0; i < weaponsAvailable.Count; i++)
		{
			anchors[i].weaponPresented = fakeWeaponsAvailable[i];
			anchors[i].SetWeaponChoice();
		}
	}

	public void ChooseWeapon(string weaponName)
	{
		RemoveAvailableWeapon(weaponName);
		
		Debug.Log("Chose " + weaponName);

		isWeaponChosen = true;
		
		if(OnWeaponChosen != null) OnWeaponChosen.Invoke(weaponName);
		
		photonView.RPC("ReceiveWeaponChosen", PhotonTargets.Others, weaponName);
		
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
	
	private void ResetWeaponsAvailable()
	{
		weaponsAvailable.Clear();
		weaponsAvailable = weaponsPool.ToList();
		fakeWeaponsAvailable = fakeWeaponsPool.ToList();
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

	public GameObject GetWeaponFromName(string name)
	{
		for (int i = 0; i < weaponsPool.Length; i++)
		{
			if (name == weaponsPool[i].name)
			{
				return weaponsPool[i];
			}
		}
		return null;
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
}
