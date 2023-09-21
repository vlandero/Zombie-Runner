using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private float range = 100f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private ParticleSystem fireFlash;
    [SerializeField] private GameObject hitImpact;

    private WeaponAmmo ammo;

    private void Start()
    {
        ammo = GetComponent<WeaponAmmo>();
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
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            EnemyAI enemy = hit.transform.GetComponent<EnemyAI>();
            enemy.EngageTarget();
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
