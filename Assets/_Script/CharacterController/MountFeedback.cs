using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountFeedback : MonoBehaviour 
{
	Animator anim;
	Vector3 _lastPosition;

	Vector3 velocity 
	{
		get
		{
			return  transform.position - _lastPosition;
		}
	}

	void Start () 
	{
		anim = GetComponent<Animator>();
	}
	
	void Update ()
	{
		anim.SetBool("Walking", velocity.magnitude > 0.01f);
		_lastPosition = transform.position;
	}
}
