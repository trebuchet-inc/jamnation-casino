using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public enum ScreenColor
{
	Neutral,
	RightColor,
	LeftColor
}

public class ScoreBoardManager : FeedbackManager
{
	public Text displayLeft, displayRight;
	public Color leftColor, rightColor;
	
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
		DisplayOnBothScreens("Welcome");
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
				ScreenColor state;

				if(msg.Contains("Blue")) state = ScreenColor.LeftColor;
				if(msg.Contains("Red")) state = ScreenColor.RightColor;
				else state = ScreenColor.Neutral;

				DisplayOnBothScreens(msg, false, state);
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

	private void DisplayOnBothScreens(string msg, bool displayScores = true, ScreenColor state = ScreenColor.Neutral)
	{
		StartCoroutine(DisplayUpdate(displayLeft, msg, displayScores,state));
		StartCoroutine(DisplayUpdate(displayRight, msg, displayScores,state));
	}

	public void DisplayUpdateOnScreen(Text display, string text)
	{
		StartCoroutine(DisplayUpdate(display, text));
	}

	private void DisplayScores()
	{
		displayLeft.color = leftColor;
		displayLeft.text = ScoreManager.Instance.GetScore(0);
		displayRight.color = rightColor;
		displayRight.text = ScoreManager.Instance.GetScore(1);
	}
	
	//
	// Coroutines
	//

	IEnumerator DisplayUpdate(Text display, string text, bool displayScores = true, ScreenColor state = ScreenColor.Neutral)
	{
		switch(state)
		{
			case ScreenColor.Neutral:
				display.color = Color.white;
			break;

			case ScreenColor.RightColor:
				display.color = rightColor;
			break;

			case ScreenColor.LeftColor:
				display.color = leftColor;
			break;
		}
		
		display.text = text;
		
		yield return new WaitForSeconds(3f);
		
		if(displayScores)DisplayScores();
	}
	
}
