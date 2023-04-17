using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;


public class LevelMatchControlOfficer : SerializedMonoBehaviour
{
    [SerializeField] LevelActor levelActor;
    [SerializeField] float matchedItemMoveDuration;
    [SerializeField] GameObject particlePrefab;
    public void MatchProcess(List<ItemActor> matchList) 
    {
        levelActor.levelCollectedPointsOfficer.CollectedStar += UIManager.instance.uICanvasOfficer.rushBarActor.collectedRushCoefficient;
        UIManager.instance.uICanvasOfficer.rushBarActor.IncreaseRushCoefficient();      
        StartCoroutine( ItemCombining(matchList));
    }

    IEnumerator ItemCombining(List<ItemActor> matchedItems)
    {
        if (matchedItems.Count >= 3)
        {
            matchedItems.Sort((t1, t2) => t1.transform.position.x.CompareTo(t2.transform.position.x));
            Transform midItem = matchedItems[1].transform;
            CreateParticle(midItem.position);
            matchedItems[0].transform.DOMove(midItem.position, matchedItemMoveDuration);
            matchedItems[2].transform.DOMove(midItem.position, matchedItemMoveDuration);

            yield return new WaitForSeconds(matchedItemMoveDuration);
            levelActor.levelItemCollectOfficer.RepositionItems();
            for (int i = matchedItems.Count - 1; i > -1; i--)
            {
                matchedItems[i].transform.DOKill();
                Destroy(matchedItems[i].gameObject, 0.1f);
            }
        }
        levelActor.levelItemCollectOfficer.RepositionItems();
    }

    void CreateParticle(Vector3 createPos) 
    {
        GameObject particle = Instantiate(particlePrefab, createPos+new Vector3(0f, 1f, 0f), Quaternion.identity, levelActor.transform);// new Vector3(0f, 1f, 0f) addition is to see particle better.
        Destroy(particle, 2f);
    }
}
