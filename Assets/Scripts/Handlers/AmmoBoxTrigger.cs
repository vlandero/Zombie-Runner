using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxTrigger : MonoBehaviour
{
    [SerializeField] private AmmoHandler ammoHandler;
    private bool canOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canOpen && !ammoHandler.GetOpened())
        {
            ammoHandler.Open();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canOpen = false;
        }
    }
}
