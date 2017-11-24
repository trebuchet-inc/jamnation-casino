using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WeaponType
{
	scourge = 0,
	axe = 1,
	spear = 2
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
	public int weaponUsed;
	public int limbHited;
	public SerializableVector3 hitPoint;

	public HitInfo (WeaponType weaponType, string limb, Vector3 hitPos)
	{
		weaponUsed = (int)weaponType;

		if(limb.Contains("Torso"))
		{
			limbHited = (int)LimbType.Torso;
		}
		else if(limb.Contains("Hand"))
		{
			limbHited = (int)LimbType.Hand;
		}
		else if(limb.Contains("Head"))
		{
			limbHited = (int)LimbType.Head;
		}
		else
		{
			limbHited = (int)LimbType.None;
		}

		hitPoint = new SerializableVector3(hitPos);
	}	
}
