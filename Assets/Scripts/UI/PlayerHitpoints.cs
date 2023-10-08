using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHitpoints : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        HealthUpdate(BalanceManager.instance.playerMaxHealth, BalanceManager.instance.playerMaxHealth);
    }
    public void HealthUpdate(float currentVal, float maxVal)
    {
        textMesh.text = currentVal + "/" + maxVal;
    }
}
