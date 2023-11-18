using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private bool hasBomb = false;

    [Header("Consumables")]
    public int revives = 0;
    public int maxRevives;
    public int garlics = 0;
    public int maxGarlics;

    [Header("Others")]
    public float hpAfterRevive;
    public float garlicEffectDuration;
    public float garlicEffectDurationAfterRevive;
    public bool immune = false;

    public int engagedZombies = 0;

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
        hpAfterRevive = BalanceManager.instance.healthAfterRevive;
        garlicEffectDuration = BalanceManager.instance.garlicEffectDuration;
        garlicEffectDurationAfterRevive = BalanceManager.instance.garlicEffectDurationAfterRevive;
        maxRevives = BalanceManager.instance.maxRevives;
        maxGarlics = BalanceManager.instance.maxGarlics;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            UseGarlic();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            UseBomb();
        }
    }

    public bool HasBomb()
    {
        return hasBomb;
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

    private IEnumerator SetImmuneUIForSeconds(float seconds)
    {
        UiManager.instance.immuneUI.SetImmune(true);
        yield return new WaitForSeconds(seconds);
        UiManager.instance.immuneUI.SetImmune(false);
    }

    private void LoseEnemyEngagement(float t)
    {
        foreach (EnemyAI enemy in GameManager.instance.enemies)
        {
            enemy.LoseAggresion(t);
        }
        StartCoroutine(SetImmuneUIForSeconds(t));
    }

    private void UseGarlic()
    {
        if (garlics == 0) return;
        garlics--;
        UiManager.instance.garlicsUI.GarlicsUpdate(garlics);
        LoseEnemyEngagement(garlicEffectDuration);
    }

    private void UseBomb()
    {
        if (!hasBomb) return;
        hasBomb = false;
        UiManager.instance.bombAvailableUI.BombAvailableUpdate(hasBomb);
        float bombRange = BalanceManager.instance.bombRange;
        Explosion explosion = playerObject.GetComponent<Explosion>();
        explosion.Explode();
        EnemyAI[] enemies = GameManager.instance.enemies.ToArray();
        foreach (EnemyAI enemy in enemies)
        {
            if (Vector3.Distance(playerObject.transform.position, enemy.transform.position) <= bombRange)
            {
                enemy.RegainInstantAggresion();
                enemy.Provoke();
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                enemyHealth.TakeDamage(BalanceManager.instance.bombDamage);
            }
        }
        TakeDamage(BalanceManager.instance.bombDamage);
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

    public void PickUpBomb()
    {
        if(hasBomb) return;
        hasBomb = true;
        UiManager.instance.bombAvailableUI.BombAvailableUpdate(hasBomb);
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
