
using System;

public class FeedbackManager : Photon.MonoBehaviour 
{
    #region variables

    // JoustHit infos
    protected string lastPlayerHitting, lastLimbHit, lastWeapon;

    #endregion
    
    protected virtual void Start()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        GameRefereeManager.Instance.OnNewGame += OnNewGameHandler;
        GameRefereeManager.Instance.OnPhaseStarted += OnPhaseStartedHandler;
        GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen += OnWeaponChosenHandler;
        GameRefereeManager.Instance.paradePhase.OnParadeReady += OnParadeReadyHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustGO += OnJoustGOHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustHit += OnJoustHitHandler;
        GameRefereeManager.Instance.endPhaseScript.OnJoustEnded += OnJoustEndedHandler; 
    }

    private void UnsubscribeEvents()
    {
        GameRefereeManager.Instance.OnNewGame -= OnNewGameHandler;
        GameRefereeManager.Instance.OnPhaseStarted -= OnPhaseStartedHandler;
        GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen -= OnWeaponChosenHandler;
        GameRefereeManager.Instance.paradePhase.OnParadeReady -= OnParadeReadyHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustGO -= OnJoustGOHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustHit -= OnJoustHitHandler;
        GameRefereeManager.Instance.endPhaseScript.OnJoustEnded -= OnJoustEndedHandler; 
    }

    protected virtual void OnNewGameHandler(){}
    
    protected virtual void OnPhaseStartedHandler(Phases phases){}

    protected virtual void OnWeaponChosenHandler(WeaponType weaponType){}

    protected virtual void OnParadeReadyHandler(){}

    protected virtual void OnJoustGOHandler(){}

    protected virtual void OnJoustHitHandler(HitInfo hitInfo)
    {
        lastPlayerHitting = ""; 
        lastLimbHit = "";
        lastWeapon = "";

        switch((LimbType)hitInfo.limbHit)
        {
            case LimbType.Head :            
                lastLimbHit = "head";
            break;

            case LimbType.Hand :            
                lastLimbHit = "hand";
            break;

            case LimbType.Torso :            
                lastLimbHit = "torso";
            break;

            case LimbType.None :
            break;
       }

        switch ((WeaponType)hitInfo.weaponUsed)
        {
            case WeaponType.Flail:
                lastWeapon = "flail";
                break;
            case WeaponType.Axe:
                lastWeapon = "axe";
                break;
            case WeaponType.Spear:
                lastWeapon = "spear";
                break;
        }
        
        lastPlayerHitting = hitInfo.playerHitting == 0 ? "Red Knight" : "Blue Knight"; 
    }
    
    protected virtual void OnJoustEndedHandler(){}
    
    private void OnDisable()
    {
        UnsubscribeEvents();
    }
}
