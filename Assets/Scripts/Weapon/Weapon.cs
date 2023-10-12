using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private float range = 100f;
    [SerializeField] private ParticleSystem fireFlash;
    [SerializeField] private GameObject hitImpact;

    private float damage = 25f;
    private WeaponAmmo ammo;

    private void Start()
    {
        ammo = GetComponent<WeaponAmmo>();
        damage = BalanceManager.instance.initialWeaponDamage;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(ammo.GetAmmo() > 0)
            {
                Shoot();
                ammo.RemoveAmmo(1);
            }
            // else play sound
        }
    }

    public WeaponAmmo GetAmmoComponent()
    {
        return ammo;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float newDamage)
    {
        damage = Mathf.Clamp(newDamage, BalanceManager.instance.weaponDamageLow, BalanceManager.instance.weaponDamageHigh);
        UiManager.instance.weaponDamageUI.UpdateWeaponDamage(damage);
    }

    private void Shoot()
    {
        PlayFlash();
        ProcessRaycast();
    }

    private void ProcessRaycast()
    {
        if (Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out RaycastHit hit))
        {
            CreateHitImpact(hit);
            if (!hit.transform.TryGetComponent<EnemyHealth>(out var target)) return;
            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
            enemy.RegainInstantAggresion();
            enemy.Provoke();
            target.TakeDamage(damage);

        }
        else
        {
            return;
        }
    }

    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitImpact, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 1f);
    }

    private void PlayFlash()
    {
        fireFlash.Play();
    }
}
