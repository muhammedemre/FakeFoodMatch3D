using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPreparationOfficer : MonoBehaviour
{
    [SerializeField] LevelActor levelActor;
    public int levelDuration, levelGoal;
    [SerializeField] float afterReadyDelay;
    [SerializeField] int numberOfTrios;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] List<ItemModelOfficer.ItemType> itemTypesToCreate = new List<ItemModelOfficer.ItemType>();
    [SerializeField] Transform leftWall, rightWall, topWall, bottomWall;
    [SerializeField] float minCreateHeight, maxCreateHeight;

    public void PrepareTheLevel()
    {
        UIManager.instance.uICanvasOfficer.LevelTimerUpdate(levelDuration);
        UIManager.instance.uICanvasOfficer.LevelInfoBoxUpdate(levelGoal);
        UIManager.instance.uICanvasOfficer.LevelCollectedStarsUpdate(0);
        UIManager.instance.uICanvasOfficer.LevelRushCoefficientUpdate(0);
        UIManager.instance.uICanvasOfficer.LevelCounterUpdate(levelActor.levelIndex);
        UIManager.instance.uICanvasOfficer.LevelSkillAmountsUpdate(levelActor.levelSkillOfficer.skillIdeaAmount, levelActor.levelSkillOfficer.skillUndoAmount,
            levelActor.levelSkillOfficer.skillFreezeAmount, levelActor.levelSkillOfficer.skillShuffleAmount);
        CreateTheTrios();

        //StartCoroutine(LevelIsReadyDelay());
    }

    IEnumerator LevelIsReadyDelay()
    {
        yield return new WaitForSeconds(afterReadyDelay);
        GameManager.instance.gameManagerObserverOfficer.Publish(ObserverSubjects.PostLevelInstantiate);
    }

    void CreateTheTrios() 
    {
        for (int i = 0; i < numberOfTrios; i++)
        {
            ItemModelOfficer.ItemType selectedType = DefineTheItemType(i);
            for (int i2 = 0; i2 < 3; i2++)
            {
                Vector3 createPos = new Vector3(UnityEngine.Random.Range(leftWall.position.x, rightWall.position.x),
                    UnityEngine.Random.Range(minCreateHeight, maxCreateHeight), UnityEngine.Random.Range(bottomWall.position.z, topWall.position.z));
                GameObject tempItem = Instantiate(itemPrefab, createPos, Quaternion.identity, levelActor.itemContainer);
                tempItem.GetComponent<ItemActor>().ItemModelOfficer.SelectTheModel(selectedType);
            }
        }
    }

    ItemModelOfficer.ItemType DefineTheItemType(int index) 
    {
        if (index < itemTypesToCreate.Count)
        {
            return itemTypesToCreate[index];
        }
        else
        {
            int itemTypeIndex = UnityEngine.Random.Range(0, ItemModelOfficer.ItemType.GetValues(typeof(ItemModelOfficer.ItemType)).Length);           
            return (ItemModelOfficer.ItemType)Enum.GetValues(typeof(ItemModelOfficer.ItemType)).GetValue(itemTypeIndex);
        }
    }
}
