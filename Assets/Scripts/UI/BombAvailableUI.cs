using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombAvailableUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private readonly string txt = "You have a bomb";

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        BombAvailableUpdate(false);
    }

    public void BombAvailableUpdate(bool available)
    {
        if (available)
        {
            textMesh.text = txt;
        }
        else
        {
            textMesh.text = "";
        }
    }
}
