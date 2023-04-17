using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class RushBarActor : MonoBehaviour
{
    [SerializeField] float decreaseDuration;
    [SerializeField] Image fill;
    public TextMeshProUGUI collectedRushCoefficientText;
    public int collectedRushCoefficient;
    float nextDecreaseTime = 0;

    private void Update()
    {
        if (collectedRushCoefficient > 0)
        {
            if (nextDecreaseTime >= Time.time)
            {
                RushBarVisualCalculation(nextDecreaseTime - Time.time);
            }
            else
            {
                DecreaseRushCoefficient();
            }
        }
    }

    public void IncreaseRushCoefficient() 
    {       
        nextDecreaseTime = Time.time + decreaseDuration;
        collectedRushCoefficient++;
        collectedRushCoefficientText.text = "x" + collectedRushCoefficient.ToString();
    }

    void DecreaseRushCoefficient() 
    {
        nextDecreaseTime = Time.time + decreaseDuration;
        collectedRushCoefficient--;
        collectedRushCoefficientText.text = "x"+collectedRushCoefficient.ToString();
    }

    void RushBarVisualCalculation(float leftDuration) 
    {
        float fillRate = leftDuration / decreaseDuration;
        fill.fillAmount = fillRate;
    }

    [Button("IncreaseRushCoefficient")]
    void ButtonIncreaseRushCoefficient() 
    {
        IncreaseRushCoefficient();
    }
}
