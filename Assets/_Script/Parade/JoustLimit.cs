using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class JoustLimit : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.attachedRigidbody.velocity = Vector3.zero;
			NVRPlayer.Instance.Mount._freeze = true;
		}
	}
}
