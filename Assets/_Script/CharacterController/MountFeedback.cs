using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountFeedback : MonoBehaviour 
{
	public ParticleSystem particle;

	Animator anim;
	Vector3 _lastPosition;

	bool playing;

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
		playing = velocity.magnitude > 0.01f;
		anim.SetBool("Walking", playing);

		if(!particle.isPlaying && playing) particle.Play();
		if(particle.isPlaying && !playing) particle.Stop();

		_lastPosition = transform.position;
	}
}
