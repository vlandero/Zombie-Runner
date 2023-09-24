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
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("PlayerCapsule"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
            if(playerHealth.GetHealth() == playerHealth.GetMaxHealth())
            {
                return;
            }
            playerHealth.Heal(healAmount);
            Destroy(gameObject);
        }
        if(collision.gameObject.layer == 10)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }
}
