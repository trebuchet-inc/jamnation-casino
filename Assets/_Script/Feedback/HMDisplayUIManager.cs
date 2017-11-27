using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class HMDisplayUIManager : FeedbackManager
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
	
	protected override void Start()
	{
		base.Start();
		canvasText = canvas.GetComponentInChildren<Text>();
		canvasText.text = "";
	}
	
	//
	// Event Handlers
	//

	protected override void OnPhaseStartedHandler(Phases phases)
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
				
			case Phases.Intermission:
//				Activate("Preparing for next round");
				break;
				
			case Phases.End:
				Activate("End of the Joust");
				break;
		}
	}

	protected override void OnWeaponChosenHandler(WeaponType weaponType)
	{
		Deactivate();
	}
	
	protected override void OnParadeReadyHandler()
	{
		Deactivate();
	}

	protected override void OnJoustHitHandler(HitInfo hitInfo)
	{
		base.OnJoustHitHandler(hitInfo);

		bool success = hitInfo.limbHit != (int)LimbType.None;
		string msg = "";
		
		if (success)
		{
			msg = "You hit in the " + lastLimbHit + " with the " + lastWeapon + "!";
		}
		else
		{
			msg = "You were hit in the " + lastLimbHit + " with the " + lastWeapon + "!";
		}
		
		StartCoroutine(DelayBeforeResult(msg, success));
	}
	
	//
	// Feedback functions
	//

	private void Activate(string textToDisplay)
	{
		 canvasText.text = textToDisplay;
	}
	
	private void Activate(string textToDisplay, float interval)
	{
		StartCoroutine(DisplayText(textToDisplay, interval));
	}

	private void Activate(Queue<string> textsToDisplay, float interval)
	{
		StartCoroutine(DisplayText(textsToDisplay, interval));
	}

	private void Deactivate()
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
	// Coroutines
	//

	private IEnumerator DisplayText(string text, float interval)
	{
		canvasText.text = text;
			
		yield return new WaitForSeconds(interval);
		
		Deactivate();
	}
	
	private IEnumerator DisplayText(Queue<string> texts, float interval)
	{
		int count = texts.Count;
		
		for (int i = 0; i < count; i++)
		{
			canvasText.text = texts.Dequeue();
			
			yield return new WaitForSeconds(interval);
		}
		
		Deactivate();
	}

	IEnumerator DelayBeforeResult(string text, bool success)
	{
		yield return new WaitForSeconds(2f);

		if (success)
		{
			SoundManager.Instance.WinJingle();
			canvasText.text = text;
		}
		else
		{
			SoundManager.Instance.LoseJingle();
			canvasText.text = "MISSED!";
		}
		
		yield return new WaitForSeconds(4f);
		canvasText.text = "";
	}
}
