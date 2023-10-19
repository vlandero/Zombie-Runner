using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    public void InitializeStart()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("PlayerCapsule"))
        {
            if (PlayerManager.instance.revives == PlayerManager.instance.maxRevives)
            {
                return;
            }
            PlayerManager.instance.PickUpRevive();
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 10)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}
