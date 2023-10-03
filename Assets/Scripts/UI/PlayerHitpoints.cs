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
        HealthUpdate(PlayerManager.instance.GetHealth(), PlayerManager.instance.GetMaxHealth());
    }
    public void HealthUpdate(float currentVal, float maxVal)
    {
        textMesh.text = currentVal + "/" + maxVal;
    }
}
