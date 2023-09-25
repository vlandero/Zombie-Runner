using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCrateText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("PlayerMainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        text.text = "Ammo: " + GetComponentInParent<AmmoHandler>().GetAvailableAmmo();
    }

}
