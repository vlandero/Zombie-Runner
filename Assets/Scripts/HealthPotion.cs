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
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if(playerHealth.GetHealth() == playerHealth.GetMaxHealth())
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                return;
            }
            playerHealth.Heal(healAmount);
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), false);
    }
}
