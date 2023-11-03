using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombItem : MonoBehaviour
{
    public void InitializeStart()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("PlayerCapsule"))
        {
            if (PlayerManager.instance.HasBomb())
            {
                return;
            }
            PlayerManager.instance.PickUpBomb();
            //ObjectPooler.instance.Destroy(gameObject, 0f);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 10)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}
