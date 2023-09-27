using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RevivesUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void RevivesUpdate(int revives)
    {
        textMesh.text = "Revives: " + revives.ToString();
    }
}
