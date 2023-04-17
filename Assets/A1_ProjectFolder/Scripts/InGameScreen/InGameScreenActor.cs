using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameScreenActor : MonoBehaviour
{
    public TimeCounterActor timeCounterActor;
    public GameObject inGameBottomBar, levelInfoBox, levelFinishBox, pauseBox;
    public TextMeshProUGUI levelCounterLabel, levelGoalText, collectedStarText;

    [SerializeField] TextMeshProUGUI finishCollectedStarText;
    [SerializeField] GameObject success, failed, outOfBoxes, notEnoughCollected, allCollected, reachedTheGoal, collectedsBox;

    public void InGameSratProcess() 
    {
        CleanAll();
        inGameBottomBar.SetActive(true);
    }

    public void ActivateLevelFinishScreen(LevelOperationOfficer.LevelFinishStates finishState, int collectedStar) 
    {
        CleanAll();
        levelFinishBox.SetActive(true);
        collectedsBox.SetActive(true);
        finishCollectedStarText.text = collectedStar.ToString();
        switch (finishState)
        {
            case LevelOperationOfficer.LevelFinishStates.fail_NotReachedGoal:
                failed.SetActive(true);
                notEnoughCollected.SetActive(true);
                break;
            case LevelOperationOfficer.LevelFinishStates.fail_ItemBoxIsFull:
                failed.SetActive(true);
                outOfBoxes.SetActive(true);
                break;
            case LevelOperationOfficer.LevelFinishStates.success_FinishedTheItems:
                success.SetActive(true);
                allCollected.SetActive(true);
                break;
            case LevelOperationOfficer.LevelFinishStates.success_ReachedTheGoal:
                success.SetActive(true);
                reachedTheGoal.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void CleanAll() 
    {
        success.SetActive(false);
        failed.SetActive(false);
        outOfBoxes.SetActive(false);
        notEnoughCollected.SetActive(false);
        allCollected.SetActive(false);
        reachedTheGoal.SetActive(false);
        pauseBox.SetActive(false);
        levelFinishBox.SetActive(false);
        collectedsBox.SetActive(false);
    }
}
