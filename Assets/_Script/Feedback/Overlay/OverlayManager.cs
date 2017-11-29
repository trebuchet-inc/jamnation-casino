using System;
using NewtonVR;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : FeedbackManager
{
    public static OverlayManager Instance;
    
    public Text SuperText, BlueText, RedText, RoundsText;

    public Color usedWeapon;
    
    // ANIMATORS
    
    public Animator mainTransition;
    
    public OverlayWeaponStatus[] blueWeapons;
    public OverlayWeaponStatus[] redWeapons;

    public Animator[] scorePoints;
    public Animator scoreBoardBG;

    public Animator super;

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

    protected override void OnNewGameHandler()
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
                PlayGoAnim(super);
                PlayGoAnim(mainTransition);
                PlayGoAnim(scoreBoardBG);
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
                
                photonView.RPC("OffWeapons", PhotonTargets.All, (int)MatchLogger.Instance.lastWeapons[0], (int)MatchLogger.Instance.lastWeapons[1]);
               
                break;
				
            case Phases.End:
                PlayOffAnim(super);
                PlayOffAnim(scoreBoardBG);
                DisplaySuper(ScoreManager.Instance.GetWinnerText());
                break;
        }
    }

    protected override void OnWeaponChosenHandler(WeaponType weaponType)
    {
        photonView.RPC("DisplayWeapons", PhotonTargets.All, NetworkPlayerManager.Instance.playerID, (int)weaponType);
    }
    
    protected override void OnJoustHitHandler(HitInfo hitInfo)
    {
        base.OnJoustHitHandler(hitInfo);
        
        DisplaySuper(lastPlayerHitting + " was hit in the " + lastLimbHit + " with a " + lastWeapon + "!");
        DisplayScores();
        
        PlayGoAnim(scorePoints[hitInfo.playerHitting]);
    }
    
    //
    // Feedback functions
    //

    private void DisplayRounds()
    {
        RoundsText.text = (GameRefereeManager.Instance.roundIndex + 1) + "/" + GameRefereeManager.Instance.TotalRounds;
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
            weapon.icon.color = Color.white;
        }
        
        foreach (var weapon in redWeapons)
        {
            weapon.icon.color = Color.white;
        }
    }

    private void PlayGoAnim(Animator animator)
    {
        animator.SetTrigger("go");
    }

    private void PlayOffAnim(Animator animator)
    {
        animator.SetTrigger("off");
    }
    
    //
    // PunRPC
    //

    [PunRPC]
    public void OffWeapons(int blueWeaponType, int redWeaponType)
    {
        PlayOffAnim(blueWeapons[blueWeaponType].status);
        PlayOffAnim(redWeapons[redWeaponType].status);
    }
    
    [PunRPC]
    public void DisplayWeapons(int playerID, int weaponType)
    {
        if (playerID == 0)
        {
//            blueWeapons[weaponType].icon.color = usedWeapon;
            PlayGoAnim(blueWeapons[weaponType].status);
        }
        else
        {
//            redWeapons[weaponType].icon.color = usedWeapon;
            PlayGoAnim(blueWeapons[weaponType].status);
        }
    }
    
}
