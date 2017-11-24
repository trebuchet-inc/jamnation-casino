
public class FeedbackManager : Photon.MonoBehaviour 
{
    private void Start()
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

    protected virtual void OnWeaponChosen(string s){}

    protected virtual void OnParadeReadyHandler(){}

    protected virtual void OnJoustGOHandler(){}
	
    protected virtual void OnJoustHitHandler(Hitinfo hitinfo){}
    
    private void OnDisable()
    {
        UnsubscribeEvents();
    }
}
