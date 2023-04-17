using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Linq;

public class LevelItemCollectOfficer : SerializedMonoBehaviour
{
    [SerializeField] LevelActor levelActor;
    [SerializeField] float pickedItemMoveDuration, pickTargetScale;
    public Dictionary<ItemModelOfficer.ItemType, List<ItemActor>> pickedItems = new Dictionary<ItemModelOfficer.ItemType, List<ItemActor>>();
    public List<ItemActor> itemBoxList = new List<ItemActor>();
    [ReadOnly] [SerializeField] Transform highlightItem = null;

    public void TouchToSelectAnItem(Vector2 touchPos)
    {
        ItemActor selectedItem = RayCheckToFindAnItem(touchPos);
        if (selectedItem != null)
        {
            PickTheItem(selectedItem);
        }
    }

    public void HoldToHighlightAnItem(Vector2 touchPos)
    {
        ItemActor selectedItem = RayCheckToFindAnItem(touchPos);

        if (highlightItem == null && selectedItem != null)
        {
            highlightItem = selectedItem.transform;
            selectedItem.GoHighlightPos();
        }
        
        //print("ITEM type: " + selectedItem?.ItemModelOfficer.selectedType);
    }

    public void HoldEndedToFinishHighlight() 
    {
        highlightItem.GetComponent<ItemActor>().GetBackFromHighLightPos();
        highlightItem = null;
    }

    ItemActor RayCheckToFindAnItem(Vector2 touchPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Item")
            {
                return hit.transform.GetComponent<ItemActor>();
            }
        }
        return null;
    }

    public void PickTheItem(ItemActor item)
    {
        bool matched = false;
        bool haveRelative = false;
        item.pickedTime = Time.time;
        if (itemBoxList.Count >= 7)
        {
            return;
        }

        if (!pickedItems.ContainsKey(item.ItemModelOfficer.selectedType))
        {
            pickedItems.Add(item.ItemModelOfficer.selectedType, new List<ItemActor>() { item });
        }
        else 
        {
            if (pickedItems[item.ItemModelOfficer.selectedType].Count > 0)
            {
                haveRelative = true;
            }          
            pickedItems[item.ItemModelOfficer.selectedType].Add(item);            

            if (pickedItems[item.ItemModelOfficer.selectedType].Count == 3)
            {
                matched = true;
            }

        }
        if (CheckIfOutOfBox())
        {
            levelActor.levelOperationOfficer.GoalReachCheck();
        }
        SendTheItemToTheBoard(item, matched, haveRelative);
    }

    void SendTheItemToTheBoard(ItemActor item, bool matched, bool haveRelative)
    {
        bool shiftRequired = false;
        int itemBoxAvailableIndex = FindTheAvailableIndex(ref shiftRequired);


        int FindTheAvailableIndex(ref bool shiftRequired)
        {
            if (haveRelative)
            {
                List<int> indexes = new List<int>();

                for (int i = 0; i < itemBoxList.Count; i++)
                {
                    if (itemBoxList[i].ItemModelOfficer.selectedType == item.ItemModelOfficer.selectedType)
                    {
                        indexes.Add(i);
                    }
                }
                shiftRequired = true;
                return indexes.Max() + 1;
            }
            else
            {
                return itemBoxList.Count;
            }
        }

        PlaceTheItemToItemBox(item, itemBoxAvailableIndex, shiftRequired, matched);
    }

    void PlaceTheItemToItemBox(ItemActor item, int index, bool shiftRequired, bool matched)
    {
        if (!shiftRequired)
        {
            itemBoxList.Add(item);
            ItemMoveToItemBox(item, index, matched);
        }
        else
        {
            itemBoxList.Insert(index, item);
            RepositionItems();
            ItemMoveToItemBox(item, index, matched);
        }
    }

    public void RepositionItems()
    {
        for (int i = 0; i < itemBoxList.Count; i++)
        {
            ItemMoveToItemBox(itemBoxList[i], i, false);
        }
        StartCoroutine( CheckIfAllItemsAreCollected());
    }

    void ItemMoveToItemBox(ItemActor item, int itemBoxAvailableIndex, bool matched)
    {
        Vector3 targetPos = UIManager.instance.uICanvasOfficer.itemBoxes.transform.GetChild(itemBoxAvailableIndex).position;
        item.transform.DOMove(targetPos, pickedItemMoveDuration).OnComplete(() => DisplayOnItemBoxProcess(item, matched));
        item.transform.DOScale(new Vector3(pickTargetScale, pickTargetScale, pickTargetScale), pickedItemMoveDuration);
        item.transform.DORotate(new Vector3(0, 0f, 90f), pickedItemMoveDuration);
    }

    void DisplayOnItemBoxProcess(ItemActor item, bool matched)
    {
        DisableRbAndColliders(item.gameObject);
        if (matched)
        {
            List<ItemActor> matchedList = PrepareMatchList(item.ItemModelOfficer.selectedType);
            foreach (ItemActor itemActor in matchedList)
            {
                itemBoxList.Remove(itemActor);
                pickedItems[item.ItemModelOfficer.selectedType].Remove(itemActor);
            }
            levelActor.levelMatchControlOfficer.MatchProcess(matchedList);
        }
    }

    void DisableRbAndColliders(GameObject item) 
    {
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.GetComponent<ItemActor>().EnableAndDisableCollidersOfTheObject(false);
    }

    bool CheckIfOutOfBox() 
    {
        int counter = 0;
        foreach (List<ItemActor> itemList in pickedItems.Values)
        {
            if (itemList.Count >= 3)
            {
                return false;
            }
            counter += itemList.Count;
        }
        return (counter >= 7? true : false);
    }
    
    List<ItemActor> PrepareMatchList(ItemModelOfficer.ItemType itemType) 
    {
        List<ItemActor> matchList = new List<ItemActor>();
        List<ItemActor> sortedObjects = pickedItems[itemType].OrderBy(o => o.pickedTime).ToList();
        for (int i = 0; i < 3; i++)
        {
            matchList.Add(sortedObjects[i]);
        }
        return matchList;
    }

    IEnumerator CheckIfAllItemsAreCollected() 
    {
        yield return new WaitForSeconds(0.15f); //0.15f is the destroy delay of matchDestroy

        if (levelActor.itemContainer.childCount == 0)
        {
            levelActor.levelOperationOfficer.LevelFinished(LevelOperationOfficer.LevelFinishStates.success_FinishedTheItems);
        }       
    }

}
