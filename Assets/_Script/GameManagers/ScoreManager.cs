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
	
	public void AddScore(int playerID, float multiplier)
	{
		int score = (int) (multiplier * 100);

		if (playerID == 0)
		{
			scoreBlue += score;
		}
		else
		{
			scoreRed += score;
		}
	}
	
	public void ResetScore()
	{
		scoreBlue = 0;
		scoreRed = 0;
	}
}
