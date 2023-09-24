using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    void Update()
    {
        textMesh.text = "Score : " + GameManager.instance.score.ToString();
    }
}
