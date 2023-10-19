using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garlic : MonoBehaviour
{
    public void InitializeStart()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("PlayerCapsule"))
        {
            if (PlayerManager.instance.garlics == PlayerManager.instance.maxGarlics)
            {
                return;
            }
            PlayerManager.instance.PickUpGarlic();
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 10)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}
