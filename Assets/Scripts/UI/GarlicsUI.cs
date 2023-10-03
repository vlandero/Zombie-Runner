using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GarlicsUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        GarlicsUpdate(PlayerManager.instance.garlics);
    }

    public void GarlicsUpdate(int garlics)
    {
        textMesh.text = "Garlics: " + garlics.ToString();
    }
}
