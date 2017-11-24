using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class NVRTorso : MonoBehaviour 
{
	public float height;

	Transform _head;

	void Start () 
	{
		_head = NVRPlayer.Instance.Head.transform;
		transform.localPosition = Vector3.up * height;
	}
	
	void Update () 
	{
		Quaternion rot = Quaternion.LookRotation(_head.position - transform.position, _head.forward);
		transform.rotation = rot;
	}
}
