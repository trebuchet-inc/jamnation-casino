using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	
	private void Start()
	{
		canvasText = canvas.GetComponentInChildren<Text>();
		canvasText.text = "";
	}
	
	//
	// Event Handlers
	//

	protected override void OnParadeReadyHandler()
	{
		Deactivate();
	}

	protected override void OnWeaponChosen(WeaponType weaponType)
	{
		Deactivate();
	}

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
				
				break;
				
			case Phases.End:
				
				break;
		}
	}
	
	//
	// Feedback functions
	//

	public void ShowResult(LimbType limb)
	{
		string text= "";
		bool sucess = false;

		switch(limb)
		{
			case LimbType.Head :
			text = "Wow! HIT THAT HEAD!";
			sucess = true;
			break;

			case LimbType.Hand :
			text = "You hit the knee : No more adventures for him!";
			sucess = true;
			break;

			case LimbType.Torso :
			text = "You hit the torso, it must be painful!";
			sucess = true;
			break;

			case LimbType.None :
			text = "Missed !";
			break;
		}
		StartCoroutine(DelayBeforeResult(text, sucess));
	}

	private void Activate(string textToDisplay)
	{
		 canvasText.text = textToDisplay;
	}

	private void Activate(Queue<string> textsToDisplay, float interval)
	{
		StartCoroutine(DisplayTexts(textsToDisplay, interval));
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
}
