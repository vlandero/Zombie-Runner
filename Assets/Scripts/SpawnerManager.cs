using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;
    public delegate void SpawnFunction(Vector3 position);

    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject healthPrefab;

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
            RandomSpawn(SpawnEnemy);
        }
    }

    public void SpawnEnemy(Vector3 spawnPoint)
    {
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();
        EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>();
        EnemyAttack enemyAttack = enemyObject.GetComponent<EnemyAttack>();

        // set things here depending on the enemy type (random maybe?)
        enemyAI.SetChaseRange(7f);
        enemyHealth.SetMaxHp(100f);
        enemyAttack.SetAttackDamage(10f);
    }

    public void SpawnAmmo(Vector3 spawnPoint)
    {
        GameObject ammoObject = Instantiate(ammoPrefab, spawnPoint, Quaternion.identity);
        AmmoHandler ammoHandler = ammoObject.GetComponent<AmmoHandler>();

        // set available ammo (random maybe?)
        ammoHandler.SetAvailableAmmo(10);
    }
    
    public void RandomSpawn(SpawnFunction callback)
    {
        bool spawned = false;
        int attempts = 0;
        while (!spawned && attempts < maxSpawnAttempts)
        {
            Vector3 randomPosition = new(UnityEngine.Random.Range(spawnRangeXMin, spawnRangeXMax), UnityEngine.Random.Range(spawnRangeYMin, spawnRangeYMax), UnityEngine.Random.Range(spawnRangeZMin, spawnRangeZMax));
            if (!Physics.CheckSphere(randomPosition, checkCollisionRadius))
            {
                callback(randomPosition);
                spawned = true;
            }
            attempts++;
        }
    }

    public void TriggerEvent(int eventCode, int numberOfSpawns)
    {
        switch (eventCode)
        {
            case 1:
                RandomSpawn(SpawnEnemy);
                break;
            case 2:
                RandomSpawn(SpawnEnemy);
                break;
            case 3:
                RandomSpawn(SpawnEnemy);
                break;
            default:
                break;
        }
    }
}
