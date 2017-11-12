using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class Weapon : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 20;

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
    }

    void Update()
    {
        target.localPosition = Vector3.Lerp(transform.localPosition, weaponHand.transform.localPosition, Time.deltaTime * lerpSpeed);
        target.localRotation = Quaternion.Lerp(transform.localRotation, weaponHand.transform.localRotation, Time.deltaTime * lerpSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        GameRefereeManager.Instance.joustPhase.callHit(other.name);
    }
}