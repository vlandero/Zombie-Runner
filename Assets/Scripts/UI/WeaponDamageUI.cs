using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponDamageUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        UpdateWeaponDamage(BalanceManager.instance.initialWeaponDamage);
    }

    public void UpdateWeaponDamage(float damage)
    {
        textMesh.text = "Damage: " + damage.ToString();
    }
}
