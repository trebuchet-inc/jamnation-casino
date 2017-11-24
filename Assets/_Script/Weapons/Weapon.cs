using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class Weapon : MonoBehaviour
{
    public string ID;
    public Transform target;
    public float positionLerpSpeed = 20;
    public float angularLerpSpeed = 20;
    public GameObject[] JoinedObjects;

    NVRHand _weaponHand;
    NVRHand _hand;
    NVRInteractableItem _item;
    bool _initialized;

    void Start()
    {
        if(target == null) target = transform;
        _item = GetComponent<NVRInteractableItem>();
        _item.VelocityMagic = positionLerpSpeed;
        _item.AngularVelocityMagic = angularLerpSpeed;
    }

    public void Initialize(NVRHand hand)
    {
        _initialized = true;

        _weaponHand = hand;
        transform.parent = hand.transform.parent;
        target.localPosition = _weaponHand.transform.localPosition;
        target.localRotation = _weaponHand.transform.localRotation;

        foreach(GameObject obj in JoinedObjects)
        {
            obj.transform.parent = null;
        }

        _item.BeginInteraction(_weaponHand);
        GameRefereeManager.Instance.intermissionPhase.OnRoundReset += DestroyWeapon;
    }

    void Update()
    {
        if(!_initialized) return;

        //target.localPosition = Vector3.Lerp(transform.localPosition, _weaponHand.transform.localPosition, Time.deltaTime * lerpSpeed);
        //target.localRotation = Quaternion.Lerp(transform.localRotation, _weaponHand.transform.localRotation, Time.deltaTime * lerpSpeed);
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.transform.tag == "Ennemy")
        {
            GameRefereeManager.Instance.joustPhase.callHit(other.transform.parent.name,ID, other.transform.position);
            SoundManager.Instance.PlayHit(gameObject.name);
        }

        if(!_initialized && other.transform.tag == "Hand") _hand = other.attachedRigidbody.GetComponent<NVRHand>();
	}

	private void OnTriggerStay(Collider other)
	{
		if ((object) _hand != null && !GameRefereeManager.Instance.weaponSelectionPhase.isWeaponChosen && _hand.HoldButtonDown)
		{
			GameRefereeManager.Instance.weaponSelectionPhase.ChooseWeapon(ID);
            Initialize(_hand);
			SoundManager.Instance.WeaponSelected();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(!_initialized && other.transform.tag == "Hand" && (object) _hand != null) _hand = null;
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