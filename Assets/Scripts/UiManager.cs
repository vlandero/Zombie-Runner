using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public AmmoUI ammoUI;
    public PlayerHealthBar playerHealthBar;
    public PlayerHitpoints playerHitpoints;

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

    private void Start()
    {
        ammoUI = FindAnyObjectByType<AmmoUI>();
        playerHealthBar = FindAnyObjectByType<PlayerHealthBar>();
        playerHitpoints = FindAnyObjectByType<PlayerHitpoints>();
        InitializeUi();
    }

    private void InitializeUi()
    {
        ammoUI.AmmoUpdate(FindAnyObjectByType<WeaponAmmo>().GetAmmo());
        PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();
        playerHealthBar.HealthUpdate(playerHealth.GetHealth(), playerHealth.GetMaxHealth());
        playerHitpoints.HealthUpdate(playerHealth.GetHealth(), playerHealth.GetMaxHealth());
    }
}
