using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupetPart : MonoBehaviour 
{
	Rigidbody _rb;
	BoxCollider _col;
	MeshRenderer[] _models;
	int baseLayer;

	public event Action OnKilled;
	public event Action OnRevived; 

	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_col = GetComponentInChildren<BoxCollider>();
		_models = GetComponentsInChildren<MeshRenderer>();
		baseLayer = _models[0].gameObject.layer;
	}

	public void Kill(Vector3 force)
	{
		transform.parent = null;

		_rb.isKinematic = false;
		_rb.useGravity = true;
		_col.isTrigger = false;

		foreach(MeshRenderer renderer in _models)
		{
			renderer.gameObject.layer = 0;
		}

		_rb.AddForce(force, ForceMode.Impulse);
		if(OnKilled != null) OnKilled.Invoke();
	}

	public void Revive(Transform parent)
	{
		transform.parent = parent;

		_rb.isKinematic = true;
		_rb.useGravity = false;
		_col.isTrigger = true;

		foreach(MeshRenderer renderer in _models)
		{
			renderer.gameObject.layer = baseLayer;
		}

		if(OnRevived != null) OnRevived.Invoke();
	}
}
