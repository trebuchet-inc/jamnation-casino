using UnityEngine;

public class ParadeTarget : MonoBehaviour 
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (!GameRefereeManager.Instance.paradePhase.isReady)
			{
				GameRefereeManager.Instance.paradePhase.SetReady();
			}
		}
	}
}
