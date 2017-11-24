using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class Weapon : MonoBehaviour
{
    public string ID;
    public Transform target;
    public float lerpSpeed = 20;
    public GameObject[] JoinedObjects;

    NVRHand weaponHand;
    bool _initialized;

    public void Initialize(NVRHand hand)
    {
        _initialized = true;
        if(target == null) target = transform;
        weaponHand = hand;
        transform.parent = hand.transform.parent;
        target.localPosition = weaponHand.transform.localPosition;
        target.localRotation = weaponHand.transform.localRotation;

        foreach(GameObject obj in JoinedObjects)
        {
            obj.transform.parent = null;
        }
    }

    void Update()
    {
        if(!_initialized) return;

        target.localPosition = Vector3.Lerp(transform.localPosition, weaponHand.transform.localPosition, Time.deltaTime * lerpSpeed);
        target.localRotation = Quaternion.Lerp(transform.localRotation, weaponHand.transform.localRotation, Time.deltaTime * lerpSpeed);
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.transform.tag == "Ennemy")
        {
            GameRefereeManager.Instance.joustPhase.callHit(other.transform.parent.name,ID, other.transform.position);
            SoundManager.Instance.PlayHit(gameObject.name);
        }


		weaponHand = other.attachedRigidbody.GetComponent<NVRHand>();
	}

	private void OnTriggerStay(Collider other)
	{
		if ((object) weaponHand != null && !GameRefereeManager.Instance.weaponSelectionPhase.isWeaponChosen && weaponHand.HoldButtonDown)
		{
			GameRefereeManager.Instance.weaponSelectionPhase.ChooseWeapon(ID);
            Initialize(weaponHand);
			SoundManager.Instance.WeaponSelected();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if((object) weaponHand != null) weaponHand = null;
	}

    void OnDestroy()
    {
        target = null;
        foreach(GameObject obj in JoinedObjects)
        {
            Destroy(obj);
        }
    }
}