using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponZoom : MonoBehaviour
{
    public Camera playerCamera;

    private float zoomedOutFOV = 87f;
    private float zoomedOutSensitivity = 300f;
    private float zoomedInFOV = 35f;
    private float zoomedInSensitivity = 135f;

    private MouseLook playerCam;

    private bool zoomedInToggle = false;

    private void Start()
    {
        playerCam = FindAnyObjectByType<MouseLook>();
        zoomedOutSensitivity = playerCam.mouseSensitivityX;
        zoomedInSensitivity = zoomedOutSensitivity * .55f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (zoomedInToggle == false)
            {
                playerCamera.fieldOfView = zoomedInFOV;
                playerCam.mouseSensitivityX = zoomedInSensitivity;
                playerCam.mouseSensitivityY = zoomedInSensitivity;
                zoomedInToggle = true;
            }
            else
            {
                playerCamera.fieldOfView = zoomedOutFOV;
                playerCam.mouseSensitivityX = zoomedOutSensitivity;
                playerCam.mouseSensitivityY = zoomedOutSensitivity;
                zoomedInToggle = false;
            }
        }
    }
}
