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

	protected override void OnNewGameHandler()
	{
		DisplayOnBothScreens("Welcome!");
	}

	protected override void OnPhaseStartedHandler(Phases phases)
	{
		string msg = "";
		
		switch (phases) 
		{
			case Phases.WeaponSelection:
				msg = "Choosing Weapons";
				DisplayOnBothScreens(msg);
				break;
			case Phases.Parade:
				msg = "Ready!";
				DisplayOnBothScreens(msg);
				break;
				
			case Phases.Joust:
				DisplayScores();
				break;	
				
			case Phases.Intermission:
				DisplayScores();
				break;
				
			case Phases.End:
				msg = ScoreManager.Instance.GetWinnerText();
				DisplayOnBothScreens(msg, false);
				break;
		}
	}

	protected override void OnJoustHitHandler(HitInfo hitInfo)
	{
		DisplayOnBothScreens("HIT!");
	}

	protected override void OnJoustGOHandler()
	{
		DisplayOnBothScreens("GO !");
	}
	
	//
	// Feedback Functions
	//

	private void DisplayOnBothScreens(string msg, bool displayScores = true)
	{
		StartCoroutine(DisplayUpdate(displayLeft, msg, displayScores));
		StartCoroutine(DisplayUpdate(displayRight, msg, displayScores));
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

	IEnumerator DisplayUpdate(Text display, string text, bool displayScores = true)
	{
		display.text = text;
		
		yield return new WaitForSeconds(3f);
		
		if(displayScores)DisplayScores();
	}
	
}
