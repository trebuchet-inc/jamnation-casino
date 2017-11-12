using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NewtonVR;

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
		isWeaponChosen = false;
		isEnemyWeaponChosen = false;

		NetworkPlayerManager.Instance.SetLocalPlayer();
		
		PresentWeaponChoice();
	}

	public override void TerminatePhase()
	{
		
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
			for(int j = 0; j < weaponsPool.Length; j++)
			{
				if(weaponsAvailable[i].name ==  weaponsPool[j].name)
				{
					anchors[i].dummyWeaponPrefab = fakeWeaponsPool[j];
					anchors[i].realWeaponPrefab = weaponsPool[j];
					anchors[i].SetWeaponChoice();
				}
			}
		}
	}

	public void ChooseWeapon(string weaponName)
	{
		RemoveAvailableWeapon(weaponName);
		RemoveFakeAvailableWeapon("dummy" + weaponName);
		
		Debug.Log("Chose " + weaponName);

		isWeaponChosen = true;
		EndWeaponChoice(weaponName);
		
		if(OnWeaponChosen != null) OnWeaponChosen.Invoke(weaponName);
		
		photonView.RPC("ReceiveWeaponChosen", PhotonTargets.Others, weaponName, NetworkPlayerManager.Instance.personalID);
		
		if (CheckIfPhaseComplete())
		{
			GameRefereeManager.Instance.ChangePhase(Phases.Parade);
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

	private void SetEnemyWeapon(string weaponName, int id)
	{
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

	private void EndWeaponChoice(string chosenWeapon)
	{
		foreach (var weaponChoice in weaponChoiceAnchors)
		{
			if (weaponChoice.dummyWeaponPrefab != null && weaponChoice.dummyWeaponPrefab.name != chosenWeapon)
			{
				Destroy(weaponChoice.dummy);
			}
		}
		foreach (var weaponChoice in otherWeaponChoiceAnchors)
		{
			if (weaponChoice.dummyWeaponPrefab != null && weaponChoice.dummyWeaponPrefab.name != chosenWeapon)
			{
				Destroy(weaponChoice.dummy);
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

	private void RemoveFakeAvailableWeapon(string weaponName)
	{
		foreach (var w in GetWeaponsAvailable())
		{
			if (w.name == weaponName)
			{
				fakeWeaponsAvailable.Remove(w);
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
