using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreBoardManager : FeedbackManager
{
	public Text displayLeft, displayRight;
	
	protected override void Start()
	{
		base.Start();
		DisplayScores();
	}
	
	//
	// Event Handlers
	//

	protected override void OnPhaseStartedHandler(Phases phases)
	{
		string msg = "";
		
		switch (phases) 
		{
			case Phases.WeaponSelection:
				msg = "CHOOSING WEAPONS";
				break;
			case Phases.Parade:
				msg = "READY";
				break;
				
			case Phases.Joust:
				
				break;	
				
			case Phases.Intermission:
				
				break;
				
			case Phases.End:
				
				break;
		}

		DisplayOnBothScreens(msg);
	}

	protected override void OnJoustHitHandler(LimbType limbHited)
	{
		DisplayOnBothScreens("HIT!");
	}

	protected override void OnJoustGOHandler()
	{
		StartCoroutine(DisplayUpdate(displayLeft, "GO !"));
		StartCoroutine(DisplayUpdate(displayRight, "GO !"));
	}
	
	//
	// Feedback Functions
	//

	private void DisplayOnBothScreens(string msg)
	{
		StartCoroutine(DisplayUpdate(displayLeft, msg));
		StartCoroutine(DisplayUpdate(displayRight, msg));
	}

	public void DisplayUpdateOnScreen(Text display, string text)
	{
		StartCoroutine(DisplayUpdate(display, text));
	}

	private void DisplayScores()
	{
		displayLeft.text = ScoreManager.Instance.GetScore(0);
		displayRight.text = ScoreManager.Instance.GetScore(1);
	}
	
	//
	// Coroutines
	//
	
	IEnumerator DisplayUpdate(Text display, string text)
	{
		display.text = text;
		
		yield return new WaitForSeconds(3f);
		DisplayScores();
	}
	
}
