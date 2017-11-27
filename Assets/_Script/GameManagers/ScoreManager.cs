using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance;

	public int scoreBlue, scoreRed;

	public int winnerPlayerID;
	
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

		winnerPlayerID = CheckWinner();
	}
	
	public void ResetScore()
	{
		scoreBlue = 0;
		scoreRed = 0;
	}

	private int CheckWinner()
	{
		if (scoreBlue > scoreRed)
		{
			return 0;
		}
		else if (scoreBlue < scoreRed)
		{
			return 1;
		}
		else
		{
			return 2;
		}
	}

	public string GetWinnerText()
	{
		switch (winnerPlayerID)
		{
			case 0:
				return "Blue Knight wins !";
			case 1:
				return "Red Knight wins !";
			case 2:
				return "TIE!";
				default:
				return "End of the Joust";
		}
	}
}
