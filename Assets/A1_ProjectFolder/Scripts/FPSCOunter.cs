using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCOunter : MonoBehaviour
{
    private float deltaTime = 0.0f;
    [SerializeField] TextMeshProUGUI fpsText;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fpsText.text = ((int)(1.0f / deltaTime)).ToString();
    }
}
