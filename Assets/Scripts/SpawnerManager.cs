using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private float checkCollisionRadius = 4f;
    [SerializeField] private int maxSpawnAttempts = 5;

    [Header("Spawn Range")]
    [SerializeField] private float spawnRangeXMin;
    [SerializeField] private float spawnRangeXMax;
    [SerializeField] private float spawnRangeYMin;
    [SerializeField] private float spawnRangeYMax;
    [SerializeField] private float spawnRangeZMin;
    [SerializeField] private float spawnRangeZMax;
    [SerializeField] private LayerMask groundLayer;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            RandomSpawn(EnemyPrefab);
        }
    }

    public void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPoint)
    {
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();
        EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>();
        EnemyAttack enemyAttack = enemyObject.GetComponent<EnemyAttack>();

        // set things here depending on the enemy type
        enemyAI.SetChaseRange(7f);
        enemyHealth.SetMaxHp(100f);
        enemyAttack.SetAttackDamage(10f);
    }

    public void RandomSpawn(GameObject gameObject)
    {
        bool spawned = false;
        int attempts = 0;
        while (!spawned && attempts < maxSpawnAttempts)
        {
            Vector3 randomPosition = new(UnityEngine.Random.Range(spawnRangeXMin, spawnRangeXMax), UnityEngine.Random.Range(spawnRangeYMin, spawnRangeYMax), UnityEngine.Random.Range(spawnRangeZMin, spawnRangeZMax));
            if (!Physics.CheckSphere(randomPosition, checkCollisionRadius))
            {
                SpawnEnemy(gameObject, randomPosition);
                spawned = true;
            }
            attempts++;
        }
        
    }
}
