
using System;

public class FeedbackManager : Photon.MonoBehaviour 
{
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
    }

    private void UnsubscribeEvents()
    {
        GameRefereeManager.Instance.OnNewGame -= OnNewGameHandler;
        GameRefereeManager.Instance.OnPhaseStarted -= OnPhaseStartedHandler;
        GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen -= OnWeaponChosenHandler;
        GameRefereeManager.Instance.paradePhase.OnParadeReady -= OnParadeReadyHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustGO -= OnJoustGOHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustHit -= OnJoustHitHandler;
    }

    protected virtual void OnNewGameHandler(){}
    
    protected virtual void OnPhaseStartedHandler(Phases phases){}

    protected virtual void OnWeaponChosenHandler(WeaponType weaponType){}

    protected virtual void OnParadeReadyHandler(){}

    protected virtual void OnJoustGOHandler(){}
	
    protected virtual void OnJoustHitHandler(HitInfo hitInfo){}
    
    private void OnDisable()
    {
        UnsubscribeEvents();
    }
}
