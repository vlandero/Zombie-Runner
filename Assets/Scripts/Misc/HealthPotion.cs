using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private float healAmount = 50f;

    public void SetHealAmount(float amount)
    {
        healAmount = amount;
    }

    public float GetHealAmount()
    {
        return healAmount;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("PlayerCapsule"))
        {
            if(PlayerManager.instance.GetHealth() == PlayerManager.instance.GetMaxHealth())
            {
                return;
            }
            PlayerManager.instance.Heal(healAmount);
            Destroy(gameObject);
        }
        if(collision.gameObject.layer == 10)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}
