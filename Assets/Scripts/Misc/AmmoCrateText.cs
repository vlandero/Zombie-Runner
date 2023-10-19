using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoCrateText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Camera mainCamera;
    private AmmoHandler ammoHandler;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        mainCamera = GameObject.FindGameObjectWithTag("PlayerMainCamera").GetComponent<Camera>();
        ammoHandler = GetComponentInParent<AmmoHandler>();
        text.text = "Ammo: " + ammoHandler.GetAvailableAmmo();
    }

    private void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }

    public void UpdateAmmo(int ammo)
    {
        text.text = "Ammo: " + ammo;
    }

}
