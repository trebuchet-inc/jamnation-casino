using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupetPart : MonoBehaviour 
{
	Rigidbody _rb;
	BoxCollider _col;

	public event Action OnKilled;
	public event Action OnRevived; 

	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_col = GetComponentInChildren<BoxCollider>();
	}

	public void Kill(Vector3 force)
	{
		transform.parent = null;
		_rb.isKinematic = false;
		_rb.useGravity = true;
		if(_col != null)_col.isTrigger = false;
		_rb.AddForce(force, ForceMode.Impulse);
		if(OnKilled != null) OnKilled.Invoke();
	}

	public void Revive(Transform parent)
	{
		transform.parent = parent;
		_rb.isKinematic = true;
		_rb.useGravity = false;
		if(_col != null) _col.isTrigger = true;
		if(OnRevived != null) OnRevived.Invoke();
	}
}
