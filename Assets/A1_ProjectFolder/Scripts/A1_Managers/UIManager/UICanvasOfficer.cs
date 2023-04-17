using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICanvasOfficer : MonoBehaviour
{
    public GameObject inGameScreen, landingMenuScreen;
    [SerializeField] SplashScreenActor splashScreenActor;
    public InGameScreenActor inGameScreenActor;
    public RushBarActor rushBarActor;
    [SerializeField] float splashScreenDuration;    
    public GameObject itemBoxes;

    public TextMeshProUGUI skill1Text, skill2Text, skill3Text, skill4Text;
    

    public void DisplaySplashScreen()
    {
        splashScreenActor.SplashProcess(splashScreenDuration, () => AfterSplashScreenProcess());
    }

    void AfterSplashScreenProcess()
    {
        ActivateLandingMenuScreenScreen();
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PostGameStart);
    }

    public void ActivateLandingMenuScreenScreen()
    {
        splashScreenActor.gameObject.SetActive(false);
        inGameScreen.SetActive(false);
        inGameScreenActor.inGameBottomBar.SetActive(false);
        landingMenuScreen.SetActive(true);
        GameManager.instance.currentGameScreen = GameManager.ScreenState.LandingMenuScreen;
    }

    public void ActivateInGameScreen() 
    {
        landingMenuScreen.SetActive(false);
        inGameScreen.SetActive(true);
        inGameScreen.GetComponent<InGameScreenActor>().InGameSratProcess();
        
        GameManager.instance.currentGameScreen = GameManager.ScreenState.InGameScreen;
    }

    public void LevelCounterUpdate(int levelIndex)
    {
        int normalizedLevelIndex = levelIndex + 1;
        inGameScreenActor.levelCounterLabel.text = "Lvl " + normalizedLevelIndex.ToString();
    }

    public void LevelTimerUpdate(int leftDuration) 
    {
        inGameScreenActor.timeCounterActor.DisplayTheTime(leftDuration);
    }

    public void LevelInfoBoxUpdate(int goal) 
    {
        inGameScreenActor.levelGoalText.text = goal.ToString() + " X";
    }

    public void LevelCollectedStarsUpdate(int amount) 
    {
        inGameScreenActor.collectedStarText.text = amount.ToString();
    }
    public void LevelRushCoefficientUpdate(int amount)
    {
        rushBarActor.collectedRushCoefficient = amount;
        rushBarActor.collectedRushCoefficientText.text = amount.ToString();
    }

    public void LevelSkillAmountsUpdate(int amount1, int amount2, int amount3, int amount4) 
    {
        skill1Text.text = amount1.ToString();
        skill2Text.text = amount2.ToString();
        skill3Text.text = amount3.ToString();
        skill4Text.text = amount4.ToString();
    }
}
