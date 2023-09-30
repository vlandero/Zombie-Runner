using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    private int ammo;

    private void Start()
    {
        ammo = BalanceManager.instance.initialAmmo;
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        UiManager.instance.ammoUI.AmmoUpdate(ammo);
    }

    public void RemoveAmmo(int amount)
    {
        ammo -= amount;
        UiManager.instance.ammoUI.AmmoUpdate(ammo);
    }

    public int GetAmmo()
    {
        return ammo;
    }
}
