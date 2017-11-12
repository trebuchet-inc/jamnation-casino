using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxSpecific : MonoBehaviour 
{
	public Weapon weaponTarget;
	
	void OnTriggerEnter(Collider other)
    {
        GameRefereeManager.Instance.joustPhase.callHit(other.name, weaponTarget.gameObject.name, other.transform.position);
        SoundManager.Instance.PlayHit(weaponTarget.gameObject.name);
    }
}
