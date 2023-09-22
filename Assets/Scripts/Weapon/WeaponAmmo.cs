using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    [SerializeField] private int ammo = 10;
    
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
