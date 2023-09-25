using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    public Camera playerCamera;

    private float zoomedOutFOV = 87f;
    private float zoomedOutSensitivity = 7f;
    private float zoomedInFOV = 35f;
    private float zoomedInSensitivity = 3f;

    private PlayerCam playerCam;

    private bool zoomedInToggle = false;

    private void Start()
    {
        playerCam = FindAnyObjectByType<PlayerCam>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (zoomedInToggle == false)
            {
                playerCamera.fieldOfView = zoomedInFOV;
                playerCam.sensX = zoomedInSensitivity;
                playerCam.sensY = zoomedInSensitivity;
                zoomedInToggle = true;
            }
            else
            {
                playerCamera.fieldOfView = zoomedOutFOV;
                playerCam.sensX = zoomedOutSensitivity;
                playerCam.sensY = zoomedOutSensitivity;
                zoomedInToggle = false;
            }
        }
    }
}
