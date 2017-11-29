﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class Weapon : MonoBehaviour
{
    public WeaponType type;
    public float positionLerpSpeed = 20;
    public float angularLerpSpeed = 20;
    public float dyingSpeed = 1;
    public GameObject[] JoinedObjects;

    NVRHand _weaponHand;
    NVRHand _hand;
    NVRInteractableItem _item;
    bool _initialized;
    bool _nvrinitialized;
    bool _networkWeapon = false;

    void Start()
    {
        if(!_nvrinitialized) NVRInitialize();
    }

    public void NVRInitialize()
    {
        _nvrinitialized = true;

        _item = GetComponent<NVRInteractableItem>();
        _item.VelocityMagic = positionLerpSpeed;
        _item.AngularVelocityMagic = angularLerpSpeed;
    }

    public void Initialize(NVRHand hand)
    {
        _initialized = true;

        if(!_nvrinitialized)
        {
            NVRInitialize();
            _networkWeapon = true;
        } 

        _weaponHand = hand;
        transform.position = _weaponHand.transform.position;
        transform.rotation = _weaponHand.transform.rotation;

        foreach(GameObject obj in JoinedObjects)
        {
            obj.transform.parent = null;
        }

        _item.BeginInteraction(_weaponHand);
        GameRefereeManager.Instance.intermissionPhase.OnRoundReset += DestroyWeapon;
        GameRefereeManager.Instance.joustPhase.OnJoustHit += OnHitHandler;
    }

    void OnHitHandler(HitInfo info)
	{
		if((!_networkWeapon && info.playerHitting == NetworkPlayerManager.Instance.playerID) ||
		  (_networkWeapon && info.playerHitting != NetworkPlayerManager.Instance.playerID))  return;

		_item.EndInteraction(_weaponHand);
		_item.Rigidbody.isKinematic = false;
		_item.Rigidbody.useGravity = true;
		_item.Rigidbody.AddForce(NVRPlayer.Instance.transform.forward * dyingSpeed * -1.0f, ForceMode.Impulse);
	}

	private void OnTriggerEnter(Collider other)
	{
		Hit(other);

        if(!_initialized && other.transform.tag == "Hand") _hand = other.attachedRigidbody.GetComponent<NVRHand>();
	}

	private void OnTriggerStay(Collider other)
	{
		if ((object) _hand != null && !GameRefereeManager.Instance.weaponSelectionPhase.isWeaponChosen && _hand.HoldButtonDown)
		{
			GameRefereeManager.Instance.weaponSelectionPhase.ChooseWeapon(type);
            Initialize(_hand);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(!_initialized && other.transform.tag == "Hand" && (object) _hand != null) _hand = null;
	}

    public void Hit(Collider other)
    {
       if(!_networkWeapon && other.transform.tag == "Ennemy")
        {
            HitInfo info = new HitInfo(type, other.transform.parent.name, other.transform.position, NetworkPlayerManager.Instance.playerID);
            GameRefereeManager.Instance.joustPhase.callHit(info);
        } 
    }

    void DestroyWeapon()
    {
        _initialized = false;

        _item.EndInteraction(_weaponHand);
        GameRefereeManager.Instance.intermissionPhase.OnRoundReset -= DestroyWeapon;
        GameRefereeManager.Instance.joustPhase.OnJoustHit -= OnHitHandler;

        foreach(GameObject obj in JoinedObjects)
        {
            Destroy(obj);
        }
        
        Destroy(gameObject);
    }
}