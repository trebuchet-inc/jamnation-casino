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
	public Transform head;
	public Transform targetPivot;

	public GameObject canvas;
	private Text canvasText;

	public bool isLerping = true;
	public bool isTrackingHead;
	public float LerpSpeed;
	private Quaternion initialRot;

	bool success = false;
	string resultMsg = "";

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
				initialRot = targetPivot.rotation;
				Activate("Choose your weapon !");
				break;
				
			case Phases.Parade:
				Activate("Ride your horse forward !");
				break;
				
			case Phases.Joust:
				Queue<string> msg = new Queue<string>();
				msg.Enqueue("Ready"); msg.Enqueue("Set"); msg.Enqueue("GO !");
				Activate(msg, 1);
				success = false;
				resultMsg = "";
				break;
				
			case Phases.Intermission:
				isLerping = true;
				StartCoroutine(DelayBeforeResult(resultMsg, success));	
				break;
				
			case Phases.End:
				break;
		}
	}

	protected override void OnWeaponChosenHandler(WeaponType weaponType)
	{
		Deactivate();
	}
	
	protected override void OnJoustGOHandler()
	{
		Deactivate();
		isLerping = false;
	}

	protected override void OnJoustHitHandler(HitInfo hitInfo)
	{
		base.OnJoustHitHandler(hitInfo);


		success = hitInfo.limbHit != (int)LimbType.None;
		
		if (hitInfo.playerHitting == NetworkPlayerManager.Instance.playerID)
		{
			resultMsg = "You hit in the " + lastLimbHit + " with the " + lastWeapon + "!";
		}
		else
		{
			resultMsg = "You were hit in the " + lastLimbHit + " with the " + lastWeapon + "!";
		}
	}

	protected override void OnJoustEndedHandler()
	{
		Queue<string> msg = new Queue<string>();

		string winnerText = ScoreManager.Instance.GetWinnerText();
		
		msg.Enqueue("End of the Joust");

		msg.Enqueue(winnerText);
		
		Activate(msg, 2, true);
	}

	//
	// Feedback functions
	//

	private void Activate(string textToDisplay)
	{
		canvas.SetActive(true); 
		canvasText.text = textToDisplay;
	}
	
	private void Activate(string textToDisplay, float interval, bool trackHead = false)
	{
		StartCoroutine(DisplayText(textToDisplay, interval, trackHead));
	}

	private void Activate(Queue<string> textsToDisplay, float interval, bool trackHead = false)
	{
		StartCoroutine(DisplayText(textsToDisplay, interval, trackHead));
	}

	private void Deactivate()
	{
		canvasText.text = "";
		canvas.SetActive(false);
	}

	private void Update()
	{
		if (!isLerping) return;
		
		if (isTrackingHead)
		{
			Vector3 targetRot = new Vector3(0, head.transform.rotation.eulerAngles.y, 0);
			targetPivot.transform.rotation = Quaternion.Lerp(targetPivot.transform.rotation,  Quaternion.Euler(targetRot), Time.deltaTime * LerpSpeed);
		}

		canvas.transform.position = Vector3.Lerp(canvas.transform.position, UITarget.position, Time.deltaTime * LerpSpeed);
	
		canvas.transform.rotation = Quaternion.Lerp(canvas.transform.rotation, UITarget.rotation, Time.deltaTime * LerpSpeed);
	}
	
	//
	// Coroutines
	//

	private IEnumerator DisplayText(string text, float interval, bool trackHead = false)
	{
		if(trackHead) TrackHead(true);
		Activate(text);
			
		yield return new WaitForSeconds(interval);
		
		Deactivate();
		if(trackHead) TrackHead(false);
	}
	
	private IEnumerator DisplayText(Queue<string> texts, float interval, bool trackHead = false)
	{
		int count = texts.Count;
		
		if(trackHead) TrackHead(true);
		
		for (int i = 0; i < count; i++)
		{
			Activate(texts.Dequeue());
			
			yield return new WaitForSeconds(interval);
		}
		
		yield return new WaitForSeconds(interval);
		
		Deactivate();
		if(trackHead) TrackHead(false);
	}

	IEnumerator DelayBeforeResult(string text, bool success)
	{
		yield return new WaitForSeconds(0.5f);
		
		TrackHead(true);

		if (success)
		{
			CrowdManager.Instance.SetHype(4);
			SoundManager.Instance.WinJingle();
			Activate(text);
		}
		else
		{
			CrowdManager.Instance.SetHype(4, -1);
			SoundManager.Instance.LoseJingle();
			Activate("MISSED!");
		}
		
		yield return new WaitForSeconds(4f);

		TrackHead(false);
		
		Deactivate();
	}

	private void TrackHead(bool isTracking)
	{
		if (isTracking)
		{
			isTrackingHead = true;
		}
		else
		{
			isTrackingHead = false;
			targetPivot.rotation = initialRot;
		}
	}
}
