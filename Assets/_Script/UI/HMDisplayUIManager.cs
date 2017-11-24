using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HMDisplayUIManager : MonoBehaviour
{
	public static HMDisplayUIManager Instance;

	public Transform UITarget;

	public GameObject canvas;
	private Text canvasText;

	public float LerpSpeed;

	void Awake()
	{
		Instance = this;
	}
	
	

	public void ShowResult(Hitinfo info)
	{
		string text= "";
		bool sucess = false;

		switch(info)
		{
			case Hitinfo.head :
			text = "Wow! HIT THAT HEAD!";
			sucess = true;
			break;

			case Hitinfo.leg :
			text = "You hit the knee : No more adventures for him!";
			sucess = true;
			break;

			case Hitinfo.torso :
			text = "You hit the torso, it must be painful!";
			sucess = true;
			break;

			case Hitinfo.none :
			text = "Missed !";
			break;
		}
		StartCoroutine(DelayBeforeResult(text, sucess));
	}

	IEnumerator DelayBeforeResult(string text, bool sucess)
	{
		yield return new WaitForSeconds(2f);
		canvasText.text = text;

		if (sucess)
		{
			SoundManager.Instance.WinJingle();
		}
		else
		{
			SoundManager.Instance.LoseJingle();
		}
		
		yield return new WaitForSeconds(4f);
		canvasText.text = "";
	}

	private void Start()
	{
		canvasText = canvas.GetComponentInChildren<Text>();
		canvasText.text = "";
		
		GameRefereeManager.Instance.OnPhaseStarted += OnPhaseStartedHandler;
		GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen += OnWeaponChosen;
		GameRefereeManager.Instance.paradePhase.OnParadeReady += OnParadeReadyHandler;
	}

	private void OnParadeReadyHandler()
	{
		Deactivate();
	}

	private void OnWeaponChosen(string s)
	{
		Deactivate();
	}

	private void OnPhaseStartedHandler(Phases phases)
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
	}

	public void Activate(Queue<string> textsToDisplay, float interval)
	{
		StartCoroutine(DisplayTexts(textsToDisplay, interval));
	}

	public void Deactivate()
	{
		canvasText.text = "";
	}

	private void Update()
	{
		if (canvasText.text == "") return;

		canvas.transform.position = Vector3.Lerp(canvas.transform.position, UITarget.position, Time.deltaTime * LerpSpeed);
		canvas.transform.rotation = Quaternion.Lerp(canvas.transform.rotation, UITarget.rotation, Time.deltaTime * LerpSpeed);
	}
	
	//
	// COROUTINES
	//

	private IEnumerator DisplayTexts(Queue<string> texts, float interval)
	{
		int count = texts.Count;
		
		for (int i = 0; i < count; i++)
		{
			canvasText.text = texts.Dequeue();
			
			yield return new WaitForSeconds(interval);
		}
		
		Deactivate();
	}
}
