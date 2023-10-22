using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    private void Start()
    {
        SetScore(0);
    }
    public void SetScore(int score)
    {
        textMesh.text = "Score : " + score.ToString();
    }
}
