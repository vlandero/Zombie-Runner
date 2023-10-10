using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
struct SpawnRange
{
    public float spawnRangeXMin;
    public float spawnRangeXMax;
    public float spawnRangeYMin;
    public float spawnRangeYMax;
    public float spawnRangeZMin;
    public float spawnRangeZMax;
}

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager instance;
    public delegate void SpawnFunction(Vector3 position);

    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject garlicPrefab;
    [SerializeField] private GameObject revivePrefab;

    [SerializeField] private float checkCollisionRadius = 1f;
    [SerializeField] private int maxSpawnAttempts = 20;

    [Header("Spawn Range")]
    [SerializeField] private List<SpawnRange> spawnRangeList;
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

        if (Input.GetKeyDown(KeyCode.B))
        {
            RandomSpawn(SpawnHealth);
        }
    }

    private void SpawnEnemy(Vector3 spawnPoint)
    {
        GameObject enemyObject = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>();
        EnemyAttack enemyAttack = enemyObject.GetComponent<EnemyAttack>();
        EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();

        enemyHealth.SetMaxHp((int)UnityEngine.Random.Range(BalanceManager.instance.zombieMaxHealthLow, BalanceManager.instance.zombieMaxHealthHigh));
        enemyAttack.SetAttackDamage((int)UnityEngine.Random.Range(BalanceManager.instance.zombieDamageLow, BalanceManager.instance.zombieDamageHigh));
        GameManager.instance.AddEnemy(enemyAI);
    }

    private void SpawnAmmo(Vector3 spawnPoint)
    {
        GameObject ammoObject = Instantiate(ammoPrefab, spawnPoint, Quaternion.identity);
        AmmoHandler ammoHandler = ammoObject.GetComponentInChildren<AmmoHandler>();

        ammoHandler.SetAvailableAmmo(UnityEngine.Random.Range(BalanceManager.instance.ammoAmountLow, BalanceManager.instance.ammoAmountHigh));
    }

    private void SpawnHealth(Vector3 spawnPoint)
    {
        GameObject healthObject = Instantiate(healthPrefab, spawnPoint, Quaternion.identity);
        HealthPotion healthHandler = healthObject.GetComponent<HealthPotion>();
        healthHandler.SetHealAmount((int)UnityEngine.Random.Range(BalanceManager.instance.healAmountLow, BalanceManager.instance.healAmountHigh));
    }

    private void SpawnGarlic(Vector3 spawnPoint)
    {
          Instantiate(garlicPrefab, spawnPoint, Quaternion.identity);
    }

    private void SpawnRevive(Vector3 spawnPoint)
    {
        Instantiate(revivePrefab, spawnPoint, Quaternion.identity);
    }
    
    public void RandomSpawn(SpawnFunction callback)
    {
        int attempts = 0;
        while (attempts < maxSpawnAttempts)
        {
            Vector3 randomPosition = GetRandomPoint();
            if (!Physics.CheckSphere(randomPosition, checkCollisionRadius))
            {
                callback(randomPosition);
                return;
            }
            attempts++;
        }
    }

    public Vector3 GetRandomPoint()
    {
        int spawnAreaIndex = UnityEngine.Random.Range(0, spawnRangeList.Count);
        SpawnRange spawnRange = spawnRangeList[spawnAreaIndex];
        Vector3 point = new(UnityEngine.Random.Range(spawnRange.spawnRangeXMin, spawnRange.spawnRangeXMax), UnityEngine.Random.Range(spawnRange.spawnRangeYMin, spawnRange.spawnRangeYMax), UnityEngine.Random.Range(spawnRange.spawnRangeZMin, spawnRange.spawnRangeZMax));
        return point;
    }

    public void TriggerEvent(int eventCode, int rand)
    {
        switch (eventCode)
        {
            case 1:
                for(int i = 0; i < rand; ++i)
                    RandomSpawn(SpawnEnemy);
                break;
            case 2:
                for (int i = 0; i < rand; ++i)
                    RandomSpawn(SpawnAmmo);
                break;
            case 3:
                for (int i = 0; i < rand; ++i)
                    RandomSpawn(SpawnHealth);
                break;
            case 4:
                EnemyAI[] zombies = GameManager.instance.enemies.ToArray();
                for(int i = 0; i < rand; ++i)
                {
                    zombies[i].RegainInstantAggresion();
                    zombies[i].Provoke();
                }
                break;
            case 5:
                for(int i = 0; i < rand; ++i)
                    RandomSpawn(SpawnGarlic);
                break;
            case 6:
                for (int i = 0; i < rand; ++i)
                    RandomSpawn(SpawnRevive);
                break;
            default:
                break;
        }
    }
}
