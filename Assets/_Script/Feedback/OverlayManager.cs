using System;
using NewtonVR;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : FeedbackManager
{
    public static OverlayManager Instance;
    
    public Text SuperText, BlueText, RedText, RoundsText;

    public Color usedWeapon;
    
    public Image[] blueWeapons;
    public Image[] redWeapons;

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

    protected override void OnNewGame()
    {
        DisplayScores();
        DisplayRounds();
        ResetWeapons();
    }
    
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

    protected override void OnWeaponChosen(WeaponType weaponType)
    {
        photonView.RPC("DisplayWeapons", PhotonTargets.All, NetworkPlayerManager.Instance.personalID, (int)weaponType);
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


    private void ResetWeapons()
    {
        foreach (var weapon in blueWeapons)
        {
            weapon.color = Color.white;
        }
        
        foreach (var weapon in redWeapons)
        {
            weapon.color = Color.white;
        }
    }
    
    //
    // PunRPC
    //
    
    [PunRPC]
    public void DisplayWeapons(int playerID, int weaponType)
    {
        if (playerID == 0)
        {
            blueWeapons[weaponType].color = usedWeapon;
        }
        else
        {
            redWeapons[weaponType].color = usedWeapon;
        }
    }
    
}
