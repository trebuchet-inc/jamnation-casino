using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupetPart : MonoBehaviour 
{
	Rigidbody _rb;
	BoxCollider _col;

	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_col = GetComponentInChildren<BoxCollider>();
	}

	public void Kill(Vector3 force)
	{
		transform.parent = null;
		_rb.isKinematic = false;
		_col.isTrigger = false;
		_rb.AddForce(force, ForceMode.Impulse);
	}

	public void Revive(Transform parent)
	{
		transform.parent = parent;
		_rb.isKinematic = true;
		_col.isTrigger = true;
	}
}
