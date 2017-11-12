using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour 
{
	public Transform target;
	public float speed;

	Rigidbody _rb;

	void Start()
	{
		_rb = GetComponent<Rigidbody>();
	}
	
	void Update () 
	{
		Vector3 dir = target.position - transform.position;
		_rb.velocity = dir * Time.deltaTime * speed;
	}
}
