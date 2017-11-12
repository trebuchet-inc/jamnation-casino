using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class Weapon : MonoBehaviour
{
    public Transform target;

    NVRHand weaponHand;
    int lerpSpeed = 20;

    bool _initialized;

    public void Initialize(NVRHand hand)
    {
        _initialized = true;
        if(target == null) target = transform;
        weaponHand = hand;
        transform.parent = hand.transform.parent;
    }

    void Update()
    {
        target.localPosition = Vector3.Lerp(transform.localPosition, weaponHand.transform.localPosition, Time.deltaTime * 2);
        target.localRotation = Quaternion.Lerp(transform.localRotation, weaponHand.transform.localRotation, Time.deltaTime * 2);
    }
}