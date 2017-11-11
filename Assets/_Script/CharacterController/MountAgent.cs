using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;


public class MountAgent : MonoBehaviour 
{
	public GameObject mountModel;
	public NVRHand ridingHand;
	public float joustSpeed;
	public float paradeSpeed;
	public float velocityThreshold;

	float actualSpeed;

	bool _freeze = true;
	bool _mountFreeze;

	Rigidbody _rb;

	Vector3[] _deltaBuffer;
	int _index = 0;
	Vector3 _lastPosition;

	Vector3 velocity 
	{
		get
		{
			Vector3 value = Vector3.zero;
			for(int i = 0; i < _deltaBuffer.Length; i++)
			{
				value += _deltaBuffer[i];
			}
			return value;
		}
	}

	void Start () 
	{
		_rb = GetComponent<Rigidbody>();
		_lastPosition = ridingHand.transform.position;
		_deltaBuffer = new Vector3[10];

		GameRefereeManager.Instance.OnPhaseChanged += OnPhaseChangeHandler;
		GameRefereeManager.Instance.paradePhase.OnParadeReady += OnParadeReadyHandler;
	}
	
	void FixedUpdate()
	{
		_deltaBuffer[_index] = ridingHand.transform.position - _lastPosition;
		_index =(_index + 1) % _deltaBuffer.Length;
		_lastPosition = ridingHand.transform.position;
	}
	
	void Update () 
	{
		if(!_mountFreeze) 
		{
			mountModel.transform.position = transform.position;
			mountModel.transform.rotation = transform.rotation;
		}

		if(!_freeze)
		{
			if(Mathf.Abs(velocity.y) >= velocityThreshold)
			{
				_rb.AddForce(transform.forward * actualSpeed, ForceMode.Impulse);
			}
		}
	}

	void OnPhaseChangeHandler (Phases phase)
	{
		switch (phase) 
		{
			case Phases.WeaponSelection:
				_freeze = true;
			break;
				
			case Phases.Parade:
				_freeze = false;
				actualSpeed = paradeSpeed;
			break;
				
			case Phases.Joust:
				_freeze = false;
				actualSpeed = joustSpeed;
			break;	
				
			case Phases.Intermission:
				_freeze = true;
			break;
				
			case Phases.End:
				_freeze = true;
			break;
		}
	}

	public void OnParadeReadyHandler()
	{
		_freeze = true;
	}
}
