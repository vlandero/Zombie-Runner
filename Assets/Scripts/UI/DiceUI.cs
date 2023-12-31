using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EventType
{
    ZombieSpawn,
    AmmoSpawn,
    HealthSpawn,
    EngageZombies,
    SpawnGarlic,
    SpawnReviveCrystal,
    WeaponDamageChange,
    SpawnBomb
}

public class DiceUI : MonoBehaviour
{
    private readonly List<EventType> events = new List<EventType> {
        EventType.ZombieSpawn, EventType.AmmoSpawn, EventType.HealthSpawn,
        EventType.EngageZombies, EventType.SpawnGarlic, EventType.SpawnReviveCrystal,
        EventType.WeaponDamageChange, EventType.SpawnBomb
    };

    private Dictionary<EventType, int> eventToNumberMap = new Dictionary<EventType, int>();
    private Dictionary<EventType, float> eventProbabilities = new Dictionary<EventType, float>();
    private Dictionary<EventType, (int, int)> eventBoundariesMap = new Dictionary<EventType, (int, int)>();

    private TextMeshProUGUI text;
    [SerializeField] private Animator textAnimator;
    [SerializeField] private Animator diceAnimator;
    private float rollDuration = 10f;
    private float rollDelay = 20f;

    private float garlicSpawn;
    private float zombieSpawn;
    private float reviveSpawn;
    private float ammoSpawn;
    private float healthSpawn;
    private float engageZombies;
    private float weaponDamageChange;
    private float spawnGarlic;

    private void SetBoundaries()
    {
        eventBoundariesMap[EventType.ZombieSpawn] = (BalanceManager.instance.zombieSpawnLow, BalanceManager.instance.zombieSpawnHigh);
        eventBoundariesMap[EventType.AmmoSpawn] = (BalanceManager.instance.ammoSpawnLow, BalanceManager.instance.ammoSpawnHigh);
        eventBoundariesMap[EventType.HealthSpawn] = (BalanceManager.instance.healthSpawnLow, BalanceManager.instance.healthSpawnHigh);
        eventBoundariesMap[EventType.EngageZombies] = (0, 0);
        eventBoundariesMap[EventType.SpawnGarlic] = (BalanceManager.instance.garlicSpawnLow, BalanceManager.instance.garlicSpawnHigh);
        eventBoundariesMap[EventType.SpawnReviveCrystal] = (BalanceManager.instance.reviveSpawnLow, BalanceManager.instance.reviveSpawnHigh);
        eventBoundariesMap[EventType.WeaponDamageChange] = (BalanceManager.instance.weaponDamageChangeLow, BalanceManager.instance.weaponDamageChangeHigh);
    }

    private void SetProbabilities()
    {
        float sum = garlicSpawn + zombieSpawn + reviveSpawn + ammoSpawn + healthSpawn + engageZombies + weaponDamageChange + spawnGarlic;
        eventProbabilities[EventType.SpawnGarlic] = garlicSpawn / sum;
        eventProbabilities[EventType.SpawnReviveCrystal] = reviveSpawn / sum;
        eventProbabilities[EventType.HealthSpawn] = healthSpawn / sum;
        eventProbabilities[EventType.EngageZombies] = engageZombies / sum;
        eventProbabilities[EventType.ZombieSpawn] = zombieSpawn / sum;
        eventProbabilities[EventType.AmmoSpawn] = ammoSpawn / sum;
        eventProbabilities[EventType.WeaponDamageChange] = weaponDamageChange / sum;
        eventProbabilities[EventType.SpawnBomb] = spawnGarlic / sum;
    }

    private void Start()
    {
        rollDuration = BalanceManager.instance.rollDuration;
        rollDelay = BalanceManager.instance.rollCooldown;
        SetBoundaries();
        for (int i = 0; i < events.Count; i++)
        {
            eventToNumberMap[events[i]] = i + 1;
        }
        text = GetComponentInChildren<TextMeshProUGUI>();
        garlicSpawn = BalanceManager.instance.garlicSpawn;
        zombieSpawn = BalanceManager.instance.zombieSpawn;
        reviveSpawn = BalanceManager.instance.reviveSpawn;
        ammoSpawn = BalanceManager.instance.ammoSpawn;
        healthSpawn = BalanceManager.instance.healthSpawn;
        engageZombies = BalanceManager.instance.engageZombies;
        weaponDamageChange = BalanceManager.instance.weaponDamageChange;
        spawnGarlic = BalanceManager.instance.spawnGarlic;
        SetProbabilities();
        StartCoroutine(RollDicePeriodically());
    }

    private IEnumerator RollDicePeriodically()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(rollDelay);
            eventBoundariesMap[EventType.EngageZombies] = (1, GameManager.instance.GetNumberOfEnemies());
            textAnimator.SetTrigger("spin");
            diceAnimator.SetBool("spinning", true);

            float endTime = Time.time + rollDuration;
            int rand = 1;

            EventType ev = GetRandomEvent();
            while (Time.time < endTime)
            {
                ev = GetRandomEvent();
                if(ev == EventType.SpawnBomb)
                {
                    text.text = ev.ToString();
                }
                else
                {
                    rand = Random.Range(eventBoundariesMap[ev].Item1, eventBoundariesMap[ev].Item2);
                    text.text = ev.ToString() + "   " + rand.ToString();

                }
                float delay = .1f;
                yield return new WaitForSecondsRealtime(delay);
            }
            textAnimator.SetTrigger("done");
            diceAnimator.SetBool("spinning", false);
            SpawnerManager.instance.TriggerEvent(eventToNumberMap[ev], rand);
        }
    }

    private EventType GetEventFromNumber(float number)
    {
        float crt = 0;
        foreach (EventType e in events)
        {
            if (number < (crt + eventProbabilities[e]))
            {
                return e;
            }
            crt += eventProbabilities[e];
        }
        return events[^1];
    }


    private EventType GetRandomEvent()
    {
        float randomNum = Random.Range(0f, 1f);
        EventType randomEvent = GetEventFromNumber(randomNum);
        int zombiesInScene = GameManager.instance.GetNumberOfEnemies();
        int notEngagedZombies = zombiesInScene - PlayerManager.instance.engagedZombies;
        eventBoundariesMap[EventType.EngageZombies] = (1, notEngagedZombies);
        if (randomEvent == EventType.EngageZombies && notEngagedZombies == 0)
        {
            return GetRandomEvent();
        }
        return randomEvent;
    }
}
