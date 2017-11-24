using UnityEngine;

public class JoustEndTarget: MonoBehaviour
{
	public bool hasHit = false;
	
	private void Start()
	{
		GameRefereeManager.Instance.joustPhase.OnJoustHit += OnJoustHitHandler;
		GameRefereeManager.Instance.OnPhaseStarted += OnPhaseStartedHandler;
	}

	private void OnPhaseStartedHandler(Phases phases)
	{
		if (phases == Phases.Intermission) hasHit = false;
	}

	private void OnJoustHitHandler(LimbType limbHited)
	{
		hasHit = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Entered joustEnd target");

			other.attachedRigidbody.GetComponent<MountAgent>()._freeze = true;
			other.attachedRigidbody.velocity = Vector3.zero;
			
			if (!hasHit)
			{
				GameRefereeManager.Instance.joustPhase.EndJoust(false);
			}
		}
	}
}
