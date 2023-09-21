using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float hp = 100f;
    [SerializeField] private float maxHp = 100f;

    public void UpdateUI()
    {
        FindObjectOfType<PlayerHealthBar>().HealthUpdate(hp, maxHp);
        FindObjectOfType<PlayerHitpoints>().HealthUpdate(hp, maxHp);
    }
    public void TakeDamage(float damage)
    {
        hp = Mathf.Clamp(hp - damage, 0, maxHp);
        UpdateUI();
        if (hp == 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        hp = Mathf.Clamp(hp + amount, 0, maxHp);
        UpdateUI();
    }

    public float GetHealth()
    {
        return hp;
    }

    private void Die()
    {
        Debug.Log("You died");
        GetComponent<DeathHandler>().HandleDeath();
    }
}
