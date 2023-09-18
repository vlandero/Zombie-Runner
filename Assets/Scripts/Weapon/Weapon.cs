using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera firstPersonCamera;
    public float range = 100f;
    public float damage = 25f;
    public ParticleSystem fireFlash;
    public GameObject hitImpact;
    public WeaponAmmo ammo;

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
