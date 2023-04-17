using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class LevelSkillOfficer : MonoBehaviour
{
    [SerializeField] LevelActor levelActor;
    [SerializeField] float timeFreezeDuration, itemSendBackDuration, itemSendBackReleaseHeight, shuffleForcePower, shuffleForceYPower;
    public int skillIdeaAmount, skillUndoAmount, skillFreezeAmount, skillShuffleAmount;

    public IEnumerator SkillTimeFreeze() 
    {
        if (skillFreezeAmount > 0)
        {
            skillFreezeAmount--;
            UIManager.instance.uICanvasOfficer.LevelSkillAmountsUpdate(skillIdeaAmount, skillUndoAmount, skillFreezeAmount, skillShuffleAmount);

            UIManager.instance.uICanvasOfficer.inGameScreenActor.timeCounterActor.StopCounting();
            yield return new WaitForSeconds(timeFreezeDuration);
            UIManager.instance.uICanvasOfficer.inGameScreenActor.timeCounterActor.StartCountingTheTime();
        }       
    }

    public void SkillUndo() 
    {
        if (levelActor.levelItemCollectOfficer.itemBoxList.Count == 0 || skillUndoAmount <= 0)
        {
            return;
        }
        skillUndoAmount--;
        UIManager.instance.uICanvasOfficer.LevelSkillAmountsUpdate(skillIdeaAmount, skillUndoAmount, skillFreezeAmount, skillShuffleAmount);

        ItemActor lastItem = levelActor.levelItemCollectOfficer.itemBoxList[levelActor.levelItemCollectOfficer.itemBoxList.Count - 1];
        levelActor.levelItemCollectOfficer.itemBoxList.RemoveAt(levelActor.levelItemCollectOfficer.itemBoxList.Count - 1);
        levelActor.levelItemCollectOfficer.pickedItems[lastItem.ItemModelOfficer.selectedType].Remove(lastItem);
        lastItem.pickedTime = 555555;
        SendBackTheItem(lastItem);
    }

    void SendBackTheItem(ItemActor lastItem)
    {
        Transform randomItemFromContainer = LevelManager.instance.levelCreateOfficer.currentLevel.itemContainer.GetChild(UnityEngine.Random.Range(0, 
            LevelManager.instance.levelCreateOfficer.currentLevel.itemContainer.childCount));
        Vector3 sendBackPosition = randomItemFromContainer.position + new Vector3(0f, itemSendBackReleaseHeight, 0f);
        lastItem.transform.DOScale(Vector3.one, itemSendBackDuration);
        lastItem.transform.DORotate(new Vector3(UnityEngine.Random.Range(0,360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)), itemSendBackDuration);
        lastItem.EnableAndDisableCollidersOfTheObject(true);
        lastItem.transform.DOMove(sendBackPosition, itemSendBackDuration).OnComplete(()=> lastItem.GetComponent<Rigidbody>().isKinematic = false);
    }

    public void SkillIdea() 
    {
        if (skillIdeaAmount <= 0)
        {
            return;
        }
        skillIdeaAmount--;
        UIManager.instance.uICanvasOfficer.LevelSkillAmountsUpdate(skillIdeaAmount, skillUndoAmount, skillFreezeAmount, skillShuffleAmount);

        int amountOfMostCrowdedItem = 0;
        ItemModelOfficer.ItemType mostCrowdedItemType = FindTheMostCrowdedItemInItemBox(ref amountOfMostCrowdedItem);

        int requiredAmountToCreateAMatch = 3 - amountOfMostCrowdedItem;
        foreach (Transform item in levelActor.itemContainer.transform)
        {
            if (requiredAmountToCreateAMatch > 0)
            {
                if (item.GetComponent<ItemActor>().ItemModelOfficer.selectedType == mostCrowdedItemType && item.GetComponent<ItemActor>().pickedTime == 555555)
                {
                    levelActor.levelItemCollectOfficer.PickTheItem(item.GetComponent<ItemActor>());
                    requiredAmountToCreateAMatch--;
                }                
            }
            else
            {
                break;
            }
        }
    }

    ItemModelOfficer.ItemType FindTheMostCrowdedItemInItemBox(ref int amountOfMostCrowdedItem) 
    {
        int mostCrowdedPop = 0;

        ItemModelOfficer.ItemType[] values = (ItemModelOfficer.ItemType[])Enum.GetValues(typeof(ItemModelOfficer.ItemType));
        System.Random random = new System.Random();
        ItemModelOfficer.ItemType mostCrowdedItemType = values[random.Next(values.Length)];

        foreach (ItemModelOfficer.ItemType itemType in levelActor.levelItemCollectOfficer.pickedItems.Keys)
        {
            if (levelActor.levelItemCollectOfficer.pickedItems[itemType].Count > mostCrowdedPop)
            {
                mostCrowdedPop = levelActor.levelItemCollectOfficer.pickedItems[itemType].Count;
                mostCrowdedItemType = itemType;
            }
        }
        amountOfMostCrowdedItem = mostCrowdedPop;
        return mostCrowdedItemType;
    }

    public void SkillShuffle() 
    {
        if (skillShuffleAmount <= 0)
        {
            return;
        }
        skillShuffleAmount--;
        UIManager.instance.uICanvasOfficer.LevelSkillAmountsUpdate(skillIdeaAmount, skillUndoAmount, skillFreezeAmount, skillShuffleAmount);

        foreach (Transform item in levelActor.itemContainer.transform)
        {
            if (item.GetComponent<ItemActor>().pickedTime == 555555) // Shuffle this item only if its not in ItemBox
            {
                item.DOMoveY(itemSendBackReleaseHeight, itemSendBackDuration).OnComplete(() => ShuffleForce(item));
            }           
        }
    }

    void ShuffleForce(Transform item) 
    {
        Vector3 randomRotation = new Vector3(UnityEngine.Random.Range(0f, 360f), 0f, UnityEngine.Random.Range(0f, 360f));
        item.GetComponent<Rigidbody>().AddForce((randomRotation * UnityEngine.Random.Range(0, shuffleForcePower))+ new Vector3(0f, UnityEngine.Random.Range(0f, shuffleForceYPower), 0f));
    }
}
