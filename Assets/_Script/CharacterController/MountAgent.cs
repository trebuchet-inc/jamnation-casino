﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;


public class MountAgent : MonoBehaviour 
{
	public NVRHand ridingHand;
	public float speed;
	public float velocityThreshold;
	public float delay;

	float _timer = 0;
	Rigidbody _rb;

	//public

	void Start () 
	{
		_rb = GetComponent<Rigidbody>();
	}
	
	void Update () 
	{
		if(ridingHand.Rigidbody == null)return;

		_timer += Time.deltaTime;
		
		if(_timer >= delay && ridingHand.Rigidbody.velocity.y >= velocityThreshold)
		{
			_rb.AddForce(transform.forward * speed, ForceMode.Impulse);
			_timer = 0;
		}
	}

	
}
