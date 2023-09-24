using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceManager : MonoBehaviour
{
    public static BalanceManager instance;

    [Header("Initial Player Stats")]
    public int playerMaxHealth = 100;
    public int initialAmmo = 15;
    public int initialWeaponDamage = 15;

    [Header("Zombie Balance")]
    public int zombieMaxHealthLow = 50;
    public int zombieMaxHealthHigh = 120;
    public int zombieDamageLow = 5;
    public int zombieDamageHigh = 40;

    [Header("Heal Balance")]
    public int healAmountLow = 10;
    public int healAmountHigh = 100;

    [Header("Ammo Balance")]
    public int ammoAmountLow = 10;
    public int ammoAmountHigh = 20;

    public int GetZombieSpeed()
    {
        // depending on how op is the zombie
        return 0;
    }

    public int GetZombieAttackRange()
    {
          // depending on how op is the zombie
        return 0;
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
