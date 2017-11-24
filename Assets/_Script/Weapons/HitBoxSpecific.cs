using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxSpecific : MonoBehaviour 
{
	public Weapon weaponTarget;
	
	void OnTriggerEnter(Collider other)
    {
        weaponTarget.Hit(other);
    }
}
