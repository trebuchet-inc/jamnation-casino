using UnityEngine;
using System;

public enum WeaponType
{
	Flail = 0,
	Axe = 1,
	Spear = 2
}

public enum LimbType
{
	None = 0,
	Hand = 1,
	Head = 2,
	Torso = 3
}

[Serializable]
public class HitInfo
{
	public int playerHitting;
	public int weaponUsed;
	public int limbHit;
	public SerializableVector3 hitPoint;

	public HitInfo (WeaponType weaponType, string limb, Vector3 hitPos, int playerID)
	{
		weaponUsed = (int)weaponType;
		playerHitting = playerID;

		if(limb.Contains("Torso"))
		{
			limbHit = (int)LimbType.Torso;
		}
		else if(limb.Contains("Hand"))
		{
			limbHit = (int)LimbType.Hand;
		}
		else if(limb.Contains("Head"))
		{
			limbHit = (int)LimbType.Head;
		}
		else
		{
			limbHit = (int)LimbType.None;
		}

		hitPoint = new SerializableVector3(hitPos);
	}	
}
