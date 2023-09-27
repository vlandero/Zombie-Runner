using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject playerObject;

    public float moveSpeed = 5f;
    public float groundDrag = 5f;
    public float jumpForce = 4f;
    public float forceOnSlope = 10f;
    public float jumpCooldown = .25f;
    public float airMultiplier = .25f;
    public float maxSlopeAngle = 60f;

    private float hp;
    private float maxHp;

    [Header("Consumables")]
    public int revives = 0;
    public int maxRevives = 2;
    public int garlics = 0;
    public int maxGarlics = 2;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            Debug.LogError("Player GameObject not found with tag 'Player'.");
        }
    }

    private void Start()
    {
        hp = BalanceManager.instance.playerMaxHealth;
        maxHp = BalanceManager.instance.playerMaxHealth;
    }

    private void UpdateHealthUI()
    {
        UiManager.instance.playerHealthBar.HealthUpdate(hp, maxHp);
        UiManager.instance.playerHitpoints.HealthUpdate(hp, maxHp);
    }
    public void TakeDamage(float damage)
    {
        hp = Mathf.Clamp(hp - damage, 0, maxHp);
        UpdateHealthUI();
        if (hp == 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        hp = Mathf.Clamp(hp + amount, 0, maxHp);
        UpdateHealthUI();
    }

    public float GetHealth()
    {
        return hp;
    }

    public float GetMaxHealth()
    {
        return maxHp;
    }

    public void PickUpGarlic()
    {
        if (garlics == maxGarlics) return;
        garlics++;
        UiManager.instance.garlicsUI.GarlicsUpdate(garlics);
    }

    public void PickUpRevive()
    {
        if (revives == maxRevives) return;
        revives++;
        UiManager.instance.revivesUI.RevivesUpdate(revives);
    }

    private void Die()
    {
        playerObject.GetComponent<DeathHandler>().HandleDeath();
    }
}
