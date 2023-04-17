using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTouchOfficer : InputAbstractOfficer
{
    [ReadOnly] [SerializeField] float touchStartTime = 0;
    [SerializeField] float minHoldDuration;
    private void Start()
    {
        getInputOfficer.InputTouch += InputTouchProcess;
    }

    void InputTouchProcess(bool touchStart, bool touchMoved, bool touchEnded, Vector2 touchPos)
    {
        if (touchStart)
        {
            touchStartTime = Time.time;
        }
        else if (touchMoved)
        {
            if (GameManager.instance.currentGameScreen == GameManager.ScreenState.InGameScreen)
            {
                HoldCheck(touchPos);
            }
                
        }
        else if (touchEnded)
        {
            if (GameManager.instance.currentGameScreen == GameManager.ScreenState.InGameScreen)
            {
                CheckTouchEndedAsHoldOrTouch(touchPos);
            }           
        }
    }

    void HoldCheck(Vector2 touchPos) 
    {      
        float timeSinceTouchStarted = Time.time - touchStartTime;
        if (timeSinceTouchStarted >= minHoldDuration) // then its a hold
        {
            HoldOngoingProcess(touchPos);
        }        
    }

    void CheckTouchEndedAsHoldOrTouch(Vector2 touchPos) 
    {
        float timeSinceTouchStarted = Time.time - touchStartTime;
        if (timeSinceTouchStarted < minHoldDuration) // then its a Touch
        {
            TouchProcess(touchPos);
        }
        else
        {
            HoldEndProcess();
        }
    }

    void HoldEndProcess()   
    {
        LevelManager.instance.levelCreateOfficer.currentLevel.levelItemCollectOfficer.HoldEndedToFinishHighlight();
    }

    void HoldOngoingProcess(Vector2 touchPos) 
    {
        LevelManager.instance.levelCreateOfficer.currentLevel.levelItemCollectOfficer.HoldToHighlightAnItem(touchPos);
    }

    void TouchProcess(Vector2 touchPos) 
    {
        LevelManager.instance.levelCreateOfficer.currentLevel.levelItemCollectOfficer.TouchToSelectAnItem(touchPos);
    }
}
