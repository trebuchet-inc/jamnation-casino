using UnityEngine.UI;

public class OverlayManager : FeedbackManager
{
    public static OverlayManager Instance;
    
    public Text SuperText, BlueText, RedText, RoundsText;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        DisplayScores();
        DisplayRounds();
    }
    
    //
    // Event handlers
    //
    
    protected override void OnPhaseStartedHandler(Phases phases)
    {
        DisplayScores();
        DisplayRounds();
        
        switch (phases) 
        {
            case Phases.WeaponSelection:
                DisplaySuper("Jousters choosing their weapons !");
                break;
            case Phases.Parade:
                DisplaySuper("Jousters are getting ready to joust !");
                break;
				
            case Phases.Joust:
				DisplaySuper("JOUSTING !!!");
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

    private void DisplayRounds()
    {
        RoundsText.text = (GameRefereeManager.Instance.roundIndex + 1) + "/" + (GameRefereeManager.Instance.TotalRounds + 1);
    }
    
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
