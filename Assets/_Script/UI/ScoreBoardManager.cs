using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreBoardManager : FeedbackManager
{
	public static ScoreBoardManager Instance;
	
	public Text headerLeft, headerRight, displayLeft, displayRight;

	public int scoreBlue, scoreRed;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
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

		StartCoroutine(DisplayUpdate(displayLeft, msg));
		StartCoroutine(DisplayUpdate(displayRight, msg));
	}

	protected override void OnJoustHitHandler(Hitinfo hitInfo)
	{
		
	}

	protected override void OnJoustGOHandler()
	{
		StartCoroutine(DisplayUpdate(displayLeft, "GO !"));
		StartCoroutine(DisplayUpdate(displayRight, "GO !"));
	}
	
	//
	// Feedback Functions
	//

	public void AddScoreBlue(float multiplier)
	{
		scoreBlue += (int)(100 * multiplier);
	}

	public void AddScoreRed(float multiplier)
	{
		scoreRed += (int)(100 * multiplier);
	}
	
	public void ResetScore()
	{
		scoreBlue = 0;
		scoreRed = 0;
	}

	public void DisplayUpdateOnScreen(Text display, string text)
	{
		StartCoroutine(DisplayUpdate(display, text));
	}

	private void DisplayScores()
	{
		displayLeft.text = scoreBlue.ToString();
		displayRight.text = scoreRed.ToString();
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
