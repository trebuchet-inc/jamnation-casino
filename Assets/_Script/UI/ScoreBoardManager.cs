using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreBoardManager : Photon.MonoBehaviour
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
		GameRefereeManager.Instance.OnPhaseChanged += OnPhaseChangedHandler;
		
		GameRefereeManager.Instance.joustPhase.OnJoustGO += OnJoustGoHandler;
		GameRefereeManager.Instance.joustPhase.OnJoustHit += OnJoustHitHandler;
		
		DisplayScores();
	}

	public void AddScoreBlue()
	{
		scoreBlue += 100;
	}

	public void AddScoreRed()
	{
		scoreRed += 100;
	}
	
	public void ResetScore()
	{
		scoreBlue = 0;
		scoreRed = 0;
	}

	private void OnPhaseChangedHandler(Phases phases)
	{
		string msg = "";
		
		switch (phases) 
		{
			case Phases.WeaponSelection:
				msg = "CHOOSING WEAPONS";
				break;
			case Phases.Parade:
				msg = "PREPARING";
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

	private void OnJoustHitHandler(Hitinfo edfasijulghbvd)
	{
		
	}

	private void OnJoustGoHandler()
	{
		StartCoroutine(DisplayUpdate(displayLeft, "GO !"));
		StartCoroutine(DisplayUpdate(displayRight, "GO !"));
	}

	public void DisplayUpdateOnScreen(Text display, string text)
	{
		StartCoroutine(DisplayUpdate(display, text));
	}

	IEnumerator DisplayUpdate(Text display, string text)
	{
		display.text = text;
		
		yield return new WaitForSeconds(3f);
		DisplayScores();
	}

	public void DisplayScores()
	{
		displayLeft.text = scoreBlue.ToString();
		displayRight.text = scoreRed.ToString();
	}
}
