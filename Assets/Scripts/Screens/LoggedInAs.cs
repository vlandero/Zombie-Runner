using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoggedInAs : MonoBehaviour
{
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Logged in as: " + PlayerPrefs.GetString("username");
    }
}
