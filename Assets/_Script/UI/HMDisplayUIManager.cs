﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class HMDisplayUIManager : MonoBehaviour
{
	public Transform UITarget;

	public GameObject canvas;
	private Text canvasText;

	private bool isActive;

	public float LerpSpeed;

	private void Start()
	{
		canvasText = canvas.GetComponentInChildren<Text>();
		canvasText.text = "";
		
		GameRefereeManager.Instance.OnPhaseChanged += OnPhaseChangedHandler;
	}

	private void OnPhaseChangedHandler(Phases phases)
	{
	
		switch (phases) 
		{
			case Phases.WeaponSelection:
				Activate("Choose your weapon !");
				break;
				
			case Phases.Parade:
				Activate("Ride your horse to the target");
				break;
				
			case Phases.Joust:
				Queue<string> msg = new Queue<string>();
				msg.Enqueue("Ready"); msg.Enqueue("Set"); msg.Enqueue("GO !");
				Activate(msg, 1);
				break;
		}
		
	}

	public void Activate(string textToDisplay)
	{
		 canvasText.text = textToDisplay;
		 isActive = true;
	}

	public void Activate(Queue<string> textsToDisplay, float interval)
	{
		StartCoroutine(DisplayTexts(textsToDisplay, interval));
	}

	public void Deactivate()
	{
		canvasText.text = "";
		isActive = false;
	}

	private void Update()
	{
		if (!isActive) return;

		canvas.transform.position = Vector3.Lerp(canvas.transform.position, UITarget.position, Time.deltaTime * LerpSpeed);
		canvas.transform.rotation = Quaternion.Lerp(canvas.transform.rotation, UITarget.rotation, Time.deltaTime * LerpSpeed);
	}
	
	//
	// COROUTINES
	//

	private IEnumerator DisplayTexts(Queue<string> texts, float interval)
	{
		for (int i = 0; i < texts.Count; i++)
		{
			Activate(texts.Dequeue());
			
			yield return new WaitForSeconds(interval);
		}
	}
}
