using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ItemActor : MonoBehaviour
{
    public ItemModelOfficer ItemModelOfficer;
    public float pickedTime = 555555, highlightTravelDuration;
    [ReadOnly][SerializeField] Vector3 previousPos = new Vector3();
    [SerializeField] Vector3 highlightPos = new Vector3();
    [ReadOnly] [SerializeField] bool highlightOn = false;

    public void EnableAndDisableCollidersOfTheObject(bool state) 
    {
        GameObject model = ItemModelOfficer.modelList[(int)ItemModelOfficer.selectedType];
        CloseCollider(model, state);
        foreach (Transform child in model.transform)
        {
            CloseCollider(child.gameObject, state);
        }
    }

    void CloseCollider(GameObject obj, bool state)
    {
        if (obj.GetComponent<BoxCollider>())
        {
            obj.GetComponent<BoxCollider>().enabled = state;
        }
        else if (obj.GetComponent<CapsuleCollider>())
        {
            obj.GetComponent<CapsuleCollider>().enabled = state;
        }
        else if (obj.GetComponent<SphereCollider>())
        {
            obj.GetComponent<SphereCollider>().enabled = state;
        }
    }

    public void GoHighlightPos() 
    {
        if (highlightOn)
        {
            return;
        }

        previousPos = transform.position;
        transform.DOMove(highlightPos, highlightTravelDuration).OnComplete(()=> transform.GetComponent<Rigidbody>().isKinematic = true);

        highlightOn = true;
    }

    public void GetBackFromHighLightPos() 
    {
        transform.GetComponent<Rigidbody>().isKinematic = false;
        highlightOn = false;
        Vector3 getBackPos =  new Vector3(previousPos.x, highlightPos.y, previousPos.z);
        transform.DOMove(getBackPos, highlightTravelDuration/2);
    }

}
