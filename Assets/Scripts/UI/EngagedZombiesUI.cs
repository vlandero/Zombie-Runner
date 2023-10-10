using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EngagedZombiesUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        EngagedZombiesUpdate(0);
    }

    public void EngagedZombiesUpdate(int engagedZombies)
    {
        textMesh.text = "Engaged zombies: " + engagedZombies.ToString();
    }
}
