using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class MountAgent : MonoBehaviour 
{
	public GameObject mountModel;
	public ParticleSystem speedLine;
	[Space]
	public NVRHand ridingHand;
	public float ridingHandVelocityThreshold;
	public float joustSpeed;
	public float paradeSpeed;
	[Space]
	public float voiceVolumeThreshold;
	public int voiceDurationThreshold;
	public float voiceJoustSpeed;
	public float voiceParadeSpeed;
	[Space]
	public float mountDyingSpeed;

	AudioClip voice;

	float actualRidingSpeed;
	float actualVoiceSpeed;

	[HideInInspector] public bool _freeze = true;
	bool _mountFreeze;

	Rigidbody _rb;
	Rigidbody _mountRb;

	Vector3[] _deltaBuffer = new Vector3[100];
	int _index = 0;
	Vector3 _lastPosition;

	public Action OnInitialize;

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
			if(voice == null) return 0;

			int toretun = 0;
			float[] value = new float[voice.samples * voice.channels];
			voice.GetData(value, 0);
			for(int i = 0; i < value.Length; i++)
			{
				if(value[i] > voiceVolumeThreshold) toretun++;
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
		GameRefereeManager.Instance.joustPhase.OnJoustHit += OnHitHandler;
		GameRefereeManager.Instance.intermissionPhase.OnRoundReset += OnRounReset;
		DebugConsole.Instance.OnRefresh += OnDebugRefresh;
		
		mountModel = Instantiate(NetworkPlayerManager.Instance.mountPrefab, transform.position, transform.rotation);
		_mountRb = mountModel.GetComponent<Rigidbody>();

		voice = Microphone.Start(Microphone.devices[0], true, 1, 44100);
		
		if(OnInitialize != null) OnInitialize.Invoke();
	}

	void FixedUpdate()
	{
		_deltaBuffer[_index] = ridingHand.transform.position - _lastPosition;
		_index =(_index + 1) % _deltaBuffer.Length;
		_lastPosition = ridingHand.transform.position;
	}
	
	void Update () 
	{
		VoiceThresholdTweaking();

		if(!_mountFreeze) 
		{
			mountModel.transform.position = transform.position;
			mountModel.transform.rotation = transform.rotation;
		}

		if(!_freeze)
		{
			if(Mathf.Abs(velocity.y) >= ridingHandVelocityThreshold)
			{
				_rb.AddForce(transform.forward * actualRidingSpeed, ForceMode.Impulse);
				AkSoundEngine.PostEvent("Play_Horse_Rocking", gameObject);
			}

			if(voiceIntensity > voiceDurationThreshold && actualVoiceSpeed > 0)
			{
				if(!speedLine.isPlaying)speedLine.Play();
				_rb.AddForce(transform.forward * actualVoiceSpeed, ForceMode.Impulse);
			}
			else if (speedLine.isPlaying)
			{
				speedLine.Stop();
			}
		}
		else if (speedLine.isPlaying)
		{
			speedLine.Stop();
		}
	}

	void VoiceThresholdTweaking()
	{
		if(Input.GetKeyDown(KeyCode.KeypadPlus)) voiceVolumeThreshold += 0.05f;
		if(Input.GetKeyDown(KeyCode.KeypadMinus)) voiceVolumeThreshold -= 0.05f;
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
				actualRidingSpeed = paradeSpeed;
				actualVoiceSpeed = voiceParadeSpeed;
			break;	
				
			case Phases.Intermission:
				_freeze = true;
			break;
				
			case Phases.End:
				_freeze = true;
			break;
		}
	}

	void OnHitHandler(HitInfo info)
	{
		if(info.playerHitting == NetworkPlayerManager.Instance.playerID) return;

		_mountFreeze = true;
		_freeze = true;
		_mountRb.isKinematic = false;
		_mountRb.AddForce(mountModel.transform.forward * mountDyingSpeed, ForceMode.Impulse);
	}

	void OnRounReset()
	{
		_mountRb.isKinematic = true;
		_mountFreeze = false;
	}

	private void OnParadeReadyHandler()
	{
		_freeze = true;
	}

	private void OnJoustGOHandler()
	{
		_freeze = false;
		actualRidingSpeed = joustSpeed;
		actualVoiceSpeed = voiceJoustSpeed;
	}

	void OnDebugRefresh()
    {
        DebugConsole.Instance.AddEntry("Voice Treshold : " + voiceVolumeThreshold + " | " + "Voice Intensity  : " + voiceIntensity);
		DebugConsole.Instance.AddEntry(Microphone.devices[0]);
    }
}
