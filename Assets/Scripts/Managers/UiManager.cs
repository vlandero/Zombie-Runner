using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public AmmoUI ammoUI;
    public PlayerHealthBar playerHealthBar;
    public PlayerHitpoints playerHitpoints;
    public TimeElapsedHandler timeElapsedHandler;
    public GarlicsUI garlicsUI;
    public RevivesUI revivesUI;

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
        timeElapsedHandler = FindAnyObjectByType<TimeElapsedHandler>();
        garlicsUI = FindAnyObjectByType<GarlicsUI>();
        revivesUI = FindAnyObjectByType<RevivesUI>();
        InitializeUi();
    }

    private void InitializeUi()
    {
        ammoUI.AmmoUpdate(FindAnyObjectByType<WeaponAmmo>().GetAmmo());
        playerHealthBar.HealthUpdate(PlayerManager.instance.GetHealth(), PlayerManager.instance.GetMaxHealth());
        playerHitpoints.HealthUpdate(PlayerManager.instance.GetHealth(), PlayerManager.instance.GetMaxHealth());
        garlicsUI.GarlicsUpdate(PlayerManager.instance.garlics);
        revivesUI.RevivesUpdate(PlayerManager.instance.revives);
    }
}
