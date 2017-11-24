using UnityEngine.UI;

public class OverlayManager : FeedbackManager
{
    public static OverlayManager Instance;
    
    public Text SuperText, BlueText, RedText;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        DisplayScores();
    }
    
    //
    // Event handlers
    //
    
    protected override void OnPhaseStartedHandler(Phases phases)
    {
        switch (phases) 
        {
            case Phases.WeaponSelection:
                DisplaySuper("Jousters choosing their weapons !");
                break;
            case Phases.Parade:
                DisplaySuper("Jousters are getting ready to joust !");
                break;
				
            case Phases.Joust:
				
                break;	
				
            case Phases.Intermission:
                DisplaySuper("Intermission !");
                break;
				
            case Phases.End:
                DisplaySuper("This is the end for this joust ! Awesome !");
                break;
        }
    }
    
    protected override void OnJoustHitHandler(LimbType hitInfo)
    {
        DisplayScores();
    }
    
    //
    // Feedback functions
    //

    private void DisplaySuper(string text)
    {
        SuperText.text = text;
    }

    private void DisplayScores()
    {
        BlueText.text = ScoreManager.Instance.GetScore(0);
        RedText.text = ScoreManager.Instance.GetScore(1);
    }
}
