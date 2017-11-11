using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParadePhase : GamePhase
{
    public bool isReady;
    public bool isEnemyReady;

    public event Action OnReady;
    
    public override void StartPhase()
    {
        
    }

    public void SetReady()
    {
        isReady = true;
        
        if(OnReady != null) OnReady.Invoke();
        
        photonView.RPC("ReceiveSetReady", PhotonTargets.Others);

        if (CheckIfPhaseComplete())
        {
            GameRefereeManager.Instance.ChangePhase(Phases.Joust);
        }
    }

    [PunRPC]
    public void ReceiveSetReady()
    {
        isEnemyReady = true;
        
        if (CheckIfPhaseComplete())
        {
            GameRefereeManager.Instance.ChangePhase(Phases.Joust);
        }
    }

    protected override bool CheckIfPhaseComplete()
    {
        return isReady && isEnemyReady;
    }
}
