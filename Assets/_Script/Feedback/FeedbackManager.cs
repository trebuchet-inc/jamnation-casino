
public class FeedbackManager : Photon.MonoBehaviour 
{
    protected virtual void Start()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        GameRefereeManager.Instance.OnPhaseStarted += OnPhaseStartedHandler;
        GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen += OnWeaponChosen;
        GameRefereeManager.Instance.paradePhase.OnParadeReady += OnParadeReadyHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustGO += OnJoustGOHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustHit += OnJoustHitHandler;
    }

    private void UnsubscribeEvents()
    {
        GameRefereeManager.Instance.OnPhaseStarted -= OnPhaseStartedHandler;
        GameRefereeManager.Instance.weaponSelectionPhase.OnWeaponChosen -= OnWeaponChosen;
        GameRefereeManager.Instance.paradePhase.OnParadeReady -= OnParadeReadyHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustGO -= OnJoustGOHandler;
        GameRefereeManager.Instance.joustPhase.OnJoustHit -= OnJoustHitHandler;
    }
    
    protected virtual void OnPhaseStartedHandler(Phases phases){}

    protected virtual void OnWeaponChosen(WeaponType weaponType){}

    protected virtual void OnParadeReadyHandler(){}

    protected virtual void OnJoustGOHandler(){}
	
    protected virtual void OnJoustHitHandler(LimbType limbHited){}
    
    private void OnDisable()
    {
        UnsubscribeEvents();
    }
}
