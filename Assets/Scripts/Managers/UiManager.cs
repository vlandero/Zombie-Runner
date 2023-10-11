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
    public EngagedZombiesUI engagedZombiesUI;
    public ImmuneUI immuneUI;

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
        ammoUI = FindObjectOfType<AmmoUI>();
        playerHealthBar = FindObjectOfType<PlayerHealthBar>();
        playerHitpoints = FindObjectOfType<PlayerHitpoints>();
        timeElapsedHandler = FindObjectOfType<TimeElapsedHandler>();
        garlicsUI = FindObjectOfType<GarlicsUI>();
        revivesUI = FindObjectOfType<RevivesUI>();
        engagedZombiesUI = FindObjectOfType<EngagedZombiesUI>();
        immuneUI = FindObjectOfType<ImmuneUI>();
    }
}
