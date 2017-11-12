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
		Quaternion rotationDelta = target.rotation * transform.rotation;
		float angle;
        Vector3 axis;

		rotationDelta.ToAngleAxis(out angle, out axis);

		if (angle > 180)
                angle -= 360;

		if (angle != 0)
		{
			Vector3 angularTarget = angle * axis;
			if (float.IsNaN(angularTarget.x) == false)
			{
				angularTarget = angularTarget * Time.deltaTime;
				_rb.angularVelocity = Vector3.MoveTowards(this._rb.angularVelocity, angularTarget, 100);
			}
		}

		_rb.velocity = dir * Time.deltaTime * speed;
	}
}
