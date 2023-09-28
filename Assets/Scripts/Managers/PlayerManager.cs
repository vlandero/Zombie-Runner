using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject playerObject;

    [Header("Movement")]
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

    [Header("Others")]
    public float hpAfterRevive = 50f;
    public float garlicEffectDuration = 5f;
    public float garlicEffectDurationAfterRevive = 2.5f;
    public bool immune = false;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            UseGarlic();
        }
    }

    private void UpdateHealthUI()
    {
        UiManager.instance.playerHealthBar.HealthUpdate(hp, maxHp);
        UiManager.instance.playerHitpoints.HealthUpdate(hp, maxHp);
    }
    public void TakeDamage(float damage)
    {
        if (immune) return;
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

    private void LoseEnemyEngagement(float t)
    {
        foreach (EnemyAI enemy in GameManager.enemies)
        {
            enemy.LoseAggresion(t);
        }
    }

    private void UseGarlic()
    {
        if (garlics == 0) return;
        garlics--;
        UiManager.instance.garlicsUI.GarlicsUpdate(garlics);
        LoseEnemyEngagement(garlicEffectDuration);
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

    private IEnumerator MakeImmune()
    {
        immune = true;
        yield return new WaitForSecondsRealtime(1f);
        immune = false;
    }

    private void Die()
    {
        if(revives > 0)
        {
            revives--;
            UiManager.instance.revivesUI.RevivesUpdate(revives);
            LoseEnemyEngagement(garlicEffectDurationAfterRevive);
            StartCoroutine(MakeImmune());
            Heal(hpAfterRevive);
            return;
        }
        playerObject.GetComponent<DeathHandler>().HandleDeath();
    }
}
