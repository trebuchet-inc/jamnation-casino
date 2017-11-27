using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class MountAgent : MonoBehaviour 
{
	public GameObject mountModel;
	public NVRHand ridingHand;
	public float joustSpeed;
	public float voiceSpeed;
	public float paradeSpeed;
	public float velocityThreshold;
	public float voiceThreshold;
	public int voiceDurationThreshold;

	AudioClip voice;

	float actualSpeed;

	public bool _freeze = true;
	bool _mountFreeze;

	Rigidbody _rb;

	Vector3[] _deltaBuffer = new Vector3[100];
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

	int voiceIntensity 
	{
		get
		{
			int toretun = 0;
			float[] value = new float[voice.samples * voice.channels];
			voice.GetData(value, 0);
			for(int i = 0; i < value.Length; i++)
			{
				if(value[i] > voiceThreshold) toretun++;
			}
			return toretun;
		}
	}

	void Start () 
	{
		NVRPlayer.Instance.Mount = this;
		
		_rb = GetComponent<Rigidbody>();
		_lastPosition = ridingHand.transform.position;
		_deltaBuffer = new Vector3[10];

		GameRefereeManager.Instance.OnPhaseStarted += OnPhaseChangeHandler;
		GameRefereeManager.Instance.paradePhase.OnParadeReady += OnParadeReadyHandler;
		GameRefereeManager.Instance.joustPhase.OnJoustGO += OnJoustGOHandler;
		
		mountModel = Instantiate(NetworkPlayerManager.Instance.mountPrefab, transform.position, transform.rotation);

		voice = Microphone.Start(Microphone.devices[0], true, 1, 44100);
		print(Microphone.devices[0]);
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
				AkSoundEngine.PostEvent("Play_Horse_Rocking", gameObject);
			}

			if(voiceIntensity > voiceDurationThreshold)
			{
				_rb.AddForce(transform.forward * voiceSpeed, ForceMode.Impulse);
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
				
			case Phases.Intermission:
				_freeze = true;
			break;
				
			case Phases.End:
				_freeze = true;
			break;
		}
	}

	private void OnParadeReadyHandler()
	{
		_freeze = true;
	}

	private void OnJoustGOHandler()
	{
		_freeze = false;
		actualSpeed = joustSpeed;
	}
}
