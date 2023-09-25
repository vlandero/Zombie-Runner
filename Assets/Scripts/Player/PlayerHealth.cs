using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float hp;
    private float maxHp;

    private void Start()
    {
        hp = BalanceManager.instance.playerMaxHealth;
        maxHp = BalanceManager.instance.playerMaxHealth;
    }

    public void UpdateUI()
    {
        UiManager.instance.playerHealthBar.HealthUpdate(hp, maxHp);
        UiManager.instance.playerHitpoints.HealthUpdate(hp, maxHp);
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

    public float GetMaxHealth()
    {
        return maxHp;
    }

    private void Die()
    {
        Debug.Log("You died");
        GetComponent<DeathHandler>().HandleDeath();
    }
}
