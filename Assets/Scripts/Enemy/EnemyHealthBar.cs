using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    private Camera mainCamera;

    public void HealthUpdate(float currentVal, float maxVal)
    {
        healthBar.value = currentVal / maxVal;
    }

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("PlayerMainCamera").GetComponent<Camera>();
    }

    public void InstantiateStart()
    {
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        if(healthBar.value <= 0)
        {
            gameObject.SetActive(false);
        }
        if(healthBar.value <= 0.3f)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.red;
        }
        else if(healthBar.value <= 0.6f)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.green;
        }
    }
}
