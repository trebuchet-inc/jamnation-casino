using System.Collections.Generic;
using UnityEngine;

public class MatchLogger : MonoBehaviour
{
	public static MatchLogger Instance;
	
	public Stack<WeaponType>[] usedWeaponsStack = new Stack<WeaponType>[2];
	public WeaponType[] lastWeapons = new WeaponType[2];

	public WeaponType[] currentWeapons
	{
		get
		{
			return new WeaponType[2]
			{
				usedWeaponsStack[0].Peek(),
				usedWeaponsStack[1].Peek()
			};
		}
	}

	private void OnEnable()
	{
		GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen += OnWeaponChosenHandler;
	}

	private void Awake()
	{
		Instance = this;
	}

	private void OnWeaponChosenHandler(WeaponType weaponType)
	{
		LogLastWeapon(weaponType);
		usedWeaponsStack[NetworkPlayerManager.Instance.playerID].Push(weaponType);
	}

	private void LogLastWeapon(WeaponType weaponType)
	{
		lastWeapons[NetworkPlayerManager.Instance.playerID] = weaponType;
	}
}
