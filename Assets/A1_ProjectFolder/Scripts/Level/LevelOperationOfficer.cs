using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOperationOfficer : MonoBehaviour
{
    [SerializeField] LevelActor levelActor;

    public enum LevelFinishStates 
    {
        fail_NotReachedGoal, fail_ItemBoxIsFull, success_FinishedTheItems, success_ReachedTheGoal
    }

    public void LevelInfoProcess() 
    {
        UIManager.instance.uICanvasOfficer.inGameScreenActor.levelInfoBox.gameObject.SetActive(true);
    }

    public void LevelStartPlaying() 
    {
        UIManager.instance.uICanvasOfficer.inGameScreenActor.levelInfoBox.gameObject.SetActive(false);
        UIManager.instance.uICanvasOfficer.inGameScreenActor.timeCounterActor.StartCountingTheTime();
    }


    public void GoalReachCheck()
    {
        LevelFinishStates finishState = SuccessCheck() ? LevelFinishStates.success_ReachedTheGoal : LevelFinishStates.fail_NotReachedGoal;
        print("GoalReachCheck: "+finishState);
        LevelFinished(finishState);
    }


    bool SuccessCheck() 
    {
        if (levelActor.levelCollectedPointsOfficer.CollectedStar >= levelActor.levelPreparationOfficer.levelGoal)
        {
            return true;
        }
        return false;
    }

    public void LevelFinished(LevelFinishStates finishState)
    {
        int collectedStar = levelActor.levelCollectedPointsOfficer.CollectedStar;
        UIManager.instance.uICanvasOfficer.inGameScreenActor.ActivateLevelFinishScreen(finishState, collectedStar);
    }

    
}
