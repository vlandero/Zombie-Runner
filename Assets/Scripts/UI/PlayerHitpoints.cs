using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHitpoints : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }
    public void HealthUpdate(float currentVal, float maxVal)
    {
        textMesh.text = currentVal + "/" + maxVal;
    }
}
