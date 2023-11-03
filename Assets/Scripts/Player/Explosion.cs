using CartoonFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private CameraShake cameraShake;

    public void Explode()
    {
        explosion.Play();
        cameraShake.TriggerShake();
    }
}
