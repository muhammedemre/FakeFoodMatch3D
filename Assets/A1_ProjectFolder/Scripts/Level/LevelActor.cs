using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActor : MonoBehaviour
{
    public int levelIndex;
    public LevelPreparationOfficer levelPreparationOfficer;
    public LevelCollectedPointsOfficer levelCollectedPointsOfficer;
    public LevelMatchControlOfficer levelMatchControlOfficer;
    public LevelItemCollectOfficer levelItemCollectOfficer;
    public LevelOperationOfficer levelOperationOfficer;
    public LevelSkillOfficer levelSkillOfficer;

    public Transform itemContainer;
    private void Start()
    {
        levelPreparationOfficer.PrepareTheLevel();
        levelOperationOfficer.LevelInfoProcess();
    }
}
