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
    }

    void Update()
    {
        target.position = Vector3.Lerp(transform.position, weaponHand.transform.position, Time.deltaTime * 2);
        target.rotation = Quaternion.Lerp(transform.rotation, weaponHand.transform.rotation, Time.deltaTime * 2);
    }
}