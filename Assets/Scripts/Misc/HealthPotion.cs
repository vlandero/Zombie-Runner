using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private float healAmount = 50f;

    public void SetHealAmount(float amount)
    {
        healAmount = amount;
        GetComponentInChildren<HealthPotionText>().UpdateText(healAmount);
    }

    public void InitializeStart()
    {
        GetComponent<Rigidbody>().isKinematic = false;
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
            ObjectPooler.instance.Destroy(gameObject, 0f);
        }
        if(collision.gameObject.layer == 10)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
