using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class ItemModelOfficer : MonoBehaviour
{
    [SerializeField] ItemActor itemActor;
    public List<GameObject> modelList = new List<GameObject>();
    public ItemType selectedType;

    public enum ItemType 
    {
        red, green, orange, black, yellow, gray, blue, purple, pink, white
    }

    public void SelectTheModel(ItemType selectedModel)
    {
        CloseAll();
        if (modelList.Count > (int)selectedModel)
        {
            modelList[(int)selectedModel].SetActive(true);
            selectedType = selectedModel;
            gameObject.name = selectedType.ToString();
        }
    }

    void CloseAll()
    {
        foreach (GameObject model in modelList)
        {
            model.SetActive(false);
        }
    }

    #region Button

    [Title("Select Model Button")]
    [Button("Select Model", ButtonSizes.Large)]
    void ButtonSelectTheModel(ItemType selectedModel)
    {
        SelectTheModel(selectedModel);
    }
    #endregion
}
