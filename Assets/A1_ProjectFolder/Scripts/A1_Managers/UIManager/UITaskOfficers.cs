using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITaskOfficers : MonoBehaviour
{

    public void ReplayButton()
    {
        Destroy(LevelManager.instance.levelCreateOfficer.currentLevel.gameObject);
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PreLevelInstantiate);
    }

    public void HomeButton() 
    {
        Destroy(LevelManager.instance.levelCreateOfficer.currentLevel.gameObject);
        UIManager.instance.uICanvasOfficer.ActivateLandingMenuScreenScreen();
    }

    public void FoodPartyButton() 
    {
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PreLevelInstantiate);
    }

    public void Test() 
    {
        print("TEST");
    }

    public void LevelInfoButton() 
    {
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PostLevelInstantiate);
    }

    public void PauseButton() 
    {
        UIManager.instance.uICanvasOfficer.inGameScreenActor.pauseBox.SetActive(true);
    }

    public void PauseExitButton() 
    {
        UIManager.instance.uICanvasOfficer.inGameScreenActor.pauseBox.SetActive(false);
    }

    public void SkillShuffleButton() 
    {
        LevelManager.instance.levelCreateOfficer.currentLevel.levelSkillOfficer.SkillShuffle();
    }

    public void SkillIdeaButton() 
    {
        LevelManager.instance.levelCreateOfficer.currentLevel.levelSkillOfficer.SkillIdea();
    }

    public void SkillUndoButton() 
    {
        LevelManager.instance.levelCreateOfficer.currentLevel.levelSkillOfficer.SkillUndo();
    }

    public void SKillFreezeTimeButton() 
    {
        StartCoroutine(LevelManager.instance.levelCreateOfficer.currentLevel.levelSkillOfficer.SkillTimeFreeze());
    }
}
