using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCollectedPointsOfficer : MonoBehaviour
{
    [SerializeField] int collectedStar;

    public int CollectedStar   
    {
        get { return collectedStar;}
        set 
        {
            collectedStar = value;
            UIManager.instance.uICanvasOfficer.LevelCollectedStarsUpdate(collectedStar);
        }
    }
}
