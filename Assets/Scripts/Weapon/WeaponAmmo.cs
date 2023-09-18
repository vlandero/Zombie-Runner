using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmo : MonoBehaviour
{
    public int ammo = 10;
    
    public void AddAmmo(int amount)
    {
        ammo += amount;
    }

    public void RemoveAmmo(int amount)
    {
          ammo -= amount;
    }

    public int GetAmmo()
    {
        return ammo;
    }
}
