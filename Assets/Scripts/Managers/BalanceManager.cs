using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BalanceManager : MonoBehaviour
{
    public static BalanceManager instance;

    [Header("Initial Player Stats")]
    public float playerMaxHealth = 100;

    [Header("Weapon Balance")]
    public float weaponDamageLow = 10;
    public float weaponDamageHigh = 30;
    public float initialWeaponDamage = 15;
    public int initialAmmo = 15;

    [Header("Zombie Balance")]
    public float zombieMaxHealthLow = 50;
    public float zombieMaxHealthHigh = 120;
    public float zombieDamageLow = 5;
    public float zombieDamageHigh = 40;
    public float immunityTime = 5f;
    public float walkSpeedFlat = 2f;
    public float provokedSpeedFlat = 5f;
    public float chaseRangeFlat = 7f;
    public float attackRangeFlat = 3f;
    public float timeToLoseAggression = 2f;
    public float loseAggressionProbability = 5f;

    [Header("Heal Balance")]
    public float healAmountLow = 10;
    public float healAmountHigh = 100;

    [Header("Ammo Balance")]
    public int ammoAmountLow = 10;
    public int ammoAmountHigh = 20;

    [Header("Game Balance")]
    public float rollDuration = 10;
    public float rollCooldown = 20;

    [Header("Event Boundaries")]
    public int zombieSpawnLow = 3;
    public int zombieSpawnHigh = 7;
    public int healthSpawnLow = 1;
    public int healthSpawnHigh = 5;
    public int ammoSpawnLow = 1;
    public int ammoSpawnHigh = 3;
    public int garlicSpawnLow = 1;
    public int garlicSpawnHigh = 2;
    public int reviveSpawnLow = 1;
    public int reviveSpawnHigh = 2;

    [Header("Event Probability Proportion")]
    public float garlicSpawn = 1;
    public float zombieSpawn = 1;
    public float reviveSpawn = 1;
    public float ammoSpawn = 1;
    public float healthSpawn = 1;
    public float engageZombies = 1;

    public float GetZombieChaseSpeed()
    {
        return provokedSpeedFlat;
    }

    public float GetZombieWalkSpeed()
    {
        return walkSpeedFlat;
    }

    public float GetZombieAttackRange()
    {
        return attackRangeFlat;
    }

    public float GetChaseRange()
    {
        return chaseRangeFlat;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
