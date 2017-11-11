using UnityEngine;

public class GamePhase : Photon.MonoBehaviour 
{
	private void Start()
	{
		GameRefereeManager.Instance.OnNewGame += OnNewGameHandler;
	}

	protected virtual void OnNewGameHandler()
	{
		// DO SHIT WHEN GAME STARTS
	}

	public virtual void StartPhase()
	{
		
	}

	public virtual void EndPhase()
	{
		
	}

	protected virtual bool CheckIfPhaseComplete()
	{
		return false;
	}
}
