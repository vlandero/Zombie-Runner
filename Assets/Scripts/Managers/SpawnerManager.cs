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
    public delegate bool SpawnFunction(Vector3 position);

    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject garlicPrefab;
    [SerializeField] private GameObject revivePrefab;
    
    [SerializeField] private float checkCollisionRadius = 2f;
    [SerializeField] private int maxSpawnAttempts = 20;

    [Header("Spawn Range")]
    [SerializeField] private List<SpawnRange> spawnRangeList;
    [SerializeField] private LayerMask groundLayer;

    private Weapon weapon;
    private ObjectPooler objectPooler;

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

    private void Start()
    {
        weapon = PlayerManager.instance.playerObject.GetComponentInChildren<Weapon>();
        objectPooler = ObjectPooler.instance;
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

    private bool SpawnEnemy(Vector3 spawnPoint)
    {
        Debug.Log(objectPooler);
        GameObject enemyObject = objectPooler.SetActiveObjects(PoolTag.Enemy, 1).FirstOrDefault();
        if (enemyObject == null)
        {
            return false;
        }
        enemyObject.transform.position = spawnPoint;
        EnemyHealth enemyHealth = enemyObject.GetComponent<EnemyHealth>();
        EnemyAttack enemyAttack = enemyObject.GetComponent<EnemyAttack>();
        EnemyAI enemyAI = enemyObject.GetComponent<EnemyAI>();
        enemyAI.InstantiateStart();
        enemyHealth.SetMaxHp((int)UnityEngine.Random.Range(BalanceManager.instance.zombieMaxHealthLow, BalanceManager.instance.zombieMaxHealthHigh));
        enemyHealth.InstantiateStart();
        enemyAttack.SetAttackDamage((int)UnityEngine.Random.Range(BalanceManager.instance.zombieDamageLow, BalanceManager.instance.zombieDamageHigh));
        GameManager.instance.AddEnemy(enemyAI);
        return true;
    }

    private bool SpawnAmmo(Vector3 spawnPoint)
    {
        GameObject ammoObject = objectPooler.SetActiveObjects(PoolTag.Ammo, 1).FirstOrDefault();
        if (ammoObject == null) return false;

        ammoObject.transform.position = spawnPoint;
        AmmoHandler ammoHandler = ammoObject.GetComponent<AmmoHandler>();
        ammoHandler.InitializeStart();

        ammoHandler.SetAvailableAmmo(UnityEngine.Random.Range(BalanceManager.instance.ammoAmountLow, BalanceManager.instance.ammoAmountHigh));
        return true;
    }

    private bool SpawnHealth(Vector3 spawnPoint)
    {
        GameObject healthObject = objectPooler.SetActiveObjects(PoolTag.Health, 1).FirstOrDefault();
        if (healthObject == null) return false;

        healthObject.transform.position = spawnPoint;
        HealthPotion healthHandler = healthObject.GetComponent<HealthPotion>();
        healthHandler.InitializeStart();
        healthHandler.SetHealAmount((int)UnityEngine.Random.Range(BalanceManager.instance.healAmountLow, BalanceManager.instance.healAmountHigh));
        return true;
    }

    private bool SpawnGarlic(Vector3 spawnPoint)
    {
        GameObject garlicObject = objectPooler.SetActiveObjects(PoolTag.Garlic, 1).FirstOrDefault();
        if (garlicObject == null) return false;

        garlicObject.GetComponent<Garlic>().InitializeStart();
        garlicObject.transform.position = spawnPoint;
        return true;
    }

    private bool SpawnRevive(Vector3 spawnPoint)
    {
        GameObject reviveObject = objectPooler.SetActiveObjects(PoolTag.Revive, 1).FirstOrDefault();
        if (reviveObject == null) return false;

        reviveObject.GetComponent<Revive>().InitializeStart();
        reviveObject.transform.position = spawnPoint;
        return true;
    }

    public void RandomSpawn(SpawnFunction callback)
    {
        int attempts = 0;
        while (attempts < maxSpawnAttempts)
        {
            Vector3 randomPosition = GetRandomPoint();
            if (!Physics.CheckSphere(randomPosition, checkCollisionRadius))
            {
                bool wasSpawned = callback(randomPosition);
                if (wasSpawned)
                {
                    Debug.Log("Spawned");
                    return;
                }
                else
                {
                    Debug.Log("Failed to spawn, no object available in pool.");
                }
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
                int j = 0;
                int spawnedZombies = 0;
                while(spawnedZombies < rand && j < zombies.Length)
                {
                    if (!zombies[j].IsProvoked())
                    {
                        zombies[j].RegainInstantAggresion();
                        zombies[j].Provoke();
                        ++spawnedZombies;
                    }
                    ++j;
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
            case 7:
                weapon.SetDamage(weapon.GetDamage() + rand);
                break;
            default:
                break;
        }
    }
}
