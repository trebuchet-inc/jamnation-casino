using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance;

	public int scoreBlue, scoreRed;
	
	private void Awake()
	{
		Instance = this;
	}

	public string GetScore(int playerID)
	{
		return playerID == 0 ? scoreBlue.ToString() : scoreRed.ToString();
	}
	
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
}
