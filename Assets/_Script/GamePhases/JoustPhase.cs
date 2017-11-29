using System;
using System.Collections;
using UnityEngine;
using NewtonVR;

public class JoustPhase : GamePhase
{
	public event Action OnJoustGO;
	public event Action<HitInfo> OnJoustHit;

	public GameObject blood;
	public float crossingTimer = 3.0f;

	Transform opponentTransform;
	float timer;
	bool localHited;
	bool _active;

	LimbType lastHit;
	
	public override void StartPhase()
	{
		StartCoroutine(WaitForGo());
		opponentTransform = NetworkPlayerManager.Instance.players[0].transform;
		timer = crossingTimer;
		lastHit = LimbType.None;
		_active = true;
		localHited = false;
	} 
    
	public override void TerminatePhase()
	{
        _active = false;
	}

	public void EndJoust(bool hit)
	{
		GameRefereeManager.Instance.ChangePhase(Phases.Intermission);
	}

	void Update()
	{
		if(!_active) return;

		if(timer > 0 && NVRPlayer.Instance.transform.InverseTransformPoint(opponentTransform.position).z <= 0)
		{
			timer -= Time.deltaTime;
		}
		else if(timer <= 0)
		{
			EndJoust(localHited);
		}
	}

	public void callHit(HitInfo info)
	{
		if(localHited || !_active) return;

		localHited = true;
		lastHit = (LimbType)info.limbHit; 
		float multiplier = 1;

		switch(lastHit)
		{
			case LimbType.Head :
			multiplier = 3;
			break;

			case LimbType.Hand :
			multiplier = 1;
			break;
			
			case LimbType.Torso :
			multiplier = 2;
			break;
		}

		if(lastHit == LimbType.None) return;
		
		photonView.RPC("CalculateScore", PhotonTargets.All, NetworkPlayerManager.Instance.playerID, multiplier);
		photonView.RPC("ReceiveRegisterHit", PhotonTargets.Others, SerializationToolkit.ObjectToByteArray(info));
		
		if(OnJoustHit != null) OnJoustHit.Invoke(info);

		Instantiate(blood, info.hitPoint.Deserialize(), Quaternion.identity);
	}

	[PunRPC]
	public void ReceiveRegisterHit(byte[] data)
	{
		HitInfo info = (HitInfo)SerializationToolkit.ByteArrayToObject(data);
		
		if(OnJoustHit != null) OnJoustHit.Invoke(info);

		Fade.Instance.StartFade(0.4f,0.1f);
		StartCoroutine(UnFade());

		Instantiate(blood, info.hitPoint.Deserialize(), Quaternion.identity);
	}
	
	[PunRPC]
	private void CalculateScore(int playerID, float multiplier)
	{
		ScoreManager.Instance.AddScore(playerID, multiplier);
	}

	private IEnumerator UnFade()
	{
		yield return new WaitForSeconds(1.0f);

		Fade.Instance.StartFade(1f,2f);
	}

	private IEnumerator WaitForGo()
	{
		yield return new WaitForSeconds(2.5f);
		
		if(OnJoustGO != null) OnJoustGO.Invoke();
	}
}
