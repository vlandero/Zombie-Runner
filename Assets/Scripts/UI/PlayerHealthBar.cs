using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    public void HealthUpdate(float currentVal, float maxVal)
    {
        healthBar.value = currentVal / maxVal;
    }
    void Update()
    {
        if (healthBar.value <= 0)
        {
            // handle death
        }
        if (healthBar.value <= 0.3f)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.red;
        }
        else if (healthBar.value <= 0.6f)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.green;
        }
    }
}
