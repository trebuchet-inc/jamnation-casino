using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class Weapon : MonoBehaviour
{
    public WeaponType type;
    public float positionLerpSpeed = 20;
    public float angularLerpSpeed = 20;
    public GameObject[] JoinedObjects;

    NVRHand _weaponHand;
    NVRHand _hand;
    NVRInteractableItem _item;
    bool _initialized;

    void Start()
    {
        _item = GetComponent<NVRInteractableItem>();
        _item.VelocityMagic = positionLerpSpeed;
        _item.AngularVelocityMagic = angularLerpSpeed;
    }

    public void Initialize(NVRHand hand)
    {
        _initialized = true;

        _weaponHand = hand;
        transform.parent = hand.transform.parent;
        transform.localPosition = _weaponHand.transform.localPosition;
        transform.localRotation = _weaponHand.transform.localRotation;

        foreach(GameObject obj in JoinedObjects)
        {
            obj.transform.parent = null;
        }

        _item.BeginInteraction(_weaponHand);
        GameRefereeManager.Instance.intermissionPhase.OnRoundReset += DestroyWeapon;
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
			SoundManager.Instance.WeaponSelected();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(!_initialized && other.transform.tag == "Hand" && (object) _hand != null) _hand = null;
	}

    public void Hit(Collider other)
    {
       if(other.transform.tag == "Ennemy")
        {
            GameRefereeManager.Instance.joustPhase.callHit(new HitInfo(type, other.transform.parent.name, other.transform.position));
            SoundManager.Instance.PlayHit(type);
        } 
    }

    void DestroyWeapon()
    {
        _initialized = false;

        _item.EndInteraction(_weaponHand);
        GameRefereeManager.Instance.intermissionPhase.OnRoundReset -= DestroyWeapon;

        foreach(GameObject obj in JoinedObjects)
        {
            Destroy(obj);
        }
        
        Destroy(gameObject);
    }
}