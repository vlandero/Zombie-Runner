using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserError : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorText;

    public void SetError(string err)
    {
        errorText.text = err;
        errorText.color = Color.red;
    }

    public void SetText(string text)
    {
        errorText.text = text;
        errorText.color = Color.black;
    }
}
