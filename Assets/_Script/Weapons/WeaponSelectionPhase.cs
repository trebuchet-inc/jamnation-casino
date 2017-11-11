using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponSelectionPhase : GamePhase
{
	public GameObject[] weaponsPool;
	public List<GameObject> weaponsAvailable;

	public Transform[] weaponChoiceAnchors;

	public event Action<string> OnWeaponChosen;

	public GameObject currentWeapon;
	public GameObject enemyCurrentWeapon;

	public bool isWeaponChosen;
	public bool isEnemyWeaponChosen;
	
	private void Start()
	{
		GameRefereeManager.Instance.OnNewGame += OnNewGameHandler;
	}

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
		var weapons = GetWeaponsAvailable();
		for (var i = 0; i < weapons.Count; i++)
		{
			GameObject newWeapon = Instantiate(weapons[i], weaponChoiceAnchors[i].position, Quaternion.identity);
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

		currentWeapon = weapon;
		isWeaponChosen = true;
		
		if(OnWeaponChosen != null) OnWeaponChosen.Invoke(weapon.name);
		
		photonView.RPC("ReceiveWeaponChosen", PhotonTargets.Others, weapon.name);
		
		CheckIfPhaseComplete();
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
