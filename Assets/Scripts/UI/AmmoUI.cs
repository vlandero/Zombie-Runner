using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = FindObjectOfType<WeaponAmmo>().GetAmmo().ToString();
    }

    public void AmmoUpdate(int ammo)
    {
        textMesh.text = ammo.ToString();
    }
}
