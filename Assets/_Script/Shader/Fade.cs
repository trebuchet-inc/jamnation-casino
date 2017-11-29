﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {
	public static Fade Instance;
	[Range(0f,1f)] public float fade = 1;
	[Range(0f,1f)] public float grayscale = 1;
	public Color color;
	public bool isFading;

	Material _material;

	float _fadeBegin;
	float _fadeEnd;
	float _fadeSpeed;
	float _timer = 1;
	
	void Awake () 
	{
		_material = new Material( Shader.Find("Custom/Fade") );
		Instance = this;
		_material.SetColor("_Color", color);
	}

	void Start()
	{
		GameRefereeManager.Instance.joustPhase.OnJoustHit += OnHit;
		GameRefereeManager.Instance.intermissionPhase.OnRoundReset += OnReset;
	}

	void Update()
	{
		if(_timer < 1)
		{
			_timer += _fadeSpeed * Time.deltaTime;
			SetFade(_fadeBegin + _fadeEnd * _timer);
			_material.SetFloat("_Fade", fade);
			isFading = true;
		}
		else if(isFading)
		{
			isFading = false;
		}
	}

	void SetFade(float value)
	{
		fade = value;
		_material.SetFloat("_Fade", fade);
	}

	void OnHit(HitInfo info)
	{
		grayscale = 0;
		_material.SetFloat("_Grayscale", grayscale);
	}

	void OnReset()
	{
		grayscale = 1;
		_material.SetFloat("_Grayscale", grayscale);
	}

	public void StartFade(float value, float speed)
	{
		_timer = 0;
		_fadeSpeed = 1f/speed;
		_fadeBegin = fade;
		_fadeEnd = value - fade;
	}

	public void StartFade(float value, float speed, Color newColor)
	{
		_material.SetColor("_Color", newColor);
		_timer = 0;
		_fadeSpeed = 1f/speed;
		_fadeBegin = fade;
		_fadeEnd = value - fade;
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) 
	{
		Graphics.Blit (source, destination, _material);
	}
}
