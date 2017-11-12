using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
	public Text headerLeft, headerRight, displayLeft, displayRight;
	
	private void Start()
	{
		GameRefereeManager.Instance.OnPhaseChanged += OnPhaseChangedHandler;
		
		GameRefereeManager.Instance.joustPhase.OnJoustGO += OnJoustGoHandler;
		GameRefereeManager.Instance.joustPhase.OnJoustHit += OnJoustHitHandler;
	}

	private void OnPhaseChangedHandler(Phases phases)
	{
			
	}

	private void OnJoustHitHandler(Hitinfo edfasijulghbvd)
	{
		
	}

	private void OnJoustGoHandler()
	{
		
	}
}
