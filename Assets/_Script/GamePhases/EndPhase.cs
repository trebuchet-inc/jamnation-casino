﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NewtonVR;

public class EndPhase : GamePhase
{
	public int endDuration;
	public Transform[] endPositions; // 0 is winner

	private BannerColor[] banners;

	public event Action OnJoustEnded;

	private void Start()
	{
		banners = FindObjectsOfType<BannerColor>();
	}

	public override void StartPhase()
	{
		StartCoroutine(RestartTimer());
		
		photonView.RPC("SetPlayerToEndPosition", PhotonTargets.All);

		if (ScoreManager.Instance.winnerPlayerID == 0)
		{
			foreach (BannerColor banner in banners)
			{
				banner.SetBlue();
			}
		}
		else
		{
			foreach (BannerColor banner in banners)
			{
				banner.SetRed();
			}
		}
	} 
    
	public override void TerminatePhase()
	{
		foreach (BannerColor banner in banners)
		{
			banner.SetInitial();
		}
	}
	
	public IEnumerator RestartTimer()
	{
		yield return new WaitForSeconds(endDuration);
		
		if(GameRefereeManager.Instance.isFirstGame) GameRefereeManager.Instance.isFirstGame = false;
		
		GameRefereeManager.Instance.NewGame();
	}

	//
	// PunRPC
	//

	[PunRPC]
	public void SetPlayerToEndPosition()
	{
		if (NetworkPlayerManager.Instance.playerID == ScoreManager.Instance.winnerPlayerID)
		{
			NVRPlayer.Instance.transform.position = endPositions[0].position;
			NVRPlayer.Instance.transform.rotation = endPositions[0].rotation;
		}
		else
		{
			NVRPlayer.Instance.transform.position = endPositions[1].position;
			NVRPlayer.Instance.transform.rotation = endPositions[1].rotation;
		}
	}
}
