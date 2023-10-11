using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ImmuneUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void SetImmune(bool immune)
    {
        textMesh.text = immune ? "Immune" : "";
    }
}
