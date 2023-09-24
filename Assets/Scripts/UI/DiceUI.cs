using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceUI : MonoBehaviour
{
    private readonly List<string> events = new List<string>(new string[] { "Zombie Spawn", "Ammo Spawn", "Health Spawn", "Engage Zombies" });
    private readonly Dictionary<string, int> eventToNumberMap = new();
    private Dictionary<string, (int, int)> eventBoundariesMap = new();

    private TextMeshProUGUI text;
    [SerializeField] private Animator textAnimator;
    [SerializeField] private Animator diceAnimator;
    [SerializeField] private float rollDuration = 10f;
    [SerializeField] private float rollDelay = 20f;

    private int zombiesInScene = 0;

    private void SetBoundaries()
    {
        eventBoundariesMap["Zombie Spawn"] = (5, 10);
        eventBoundariesMap["Ammo Spawn"] = (1, 4);
        eventBoundariesMap["Health Spawn"] = (1, 3);
        eventBoundariesMap["Engage Zombies"] = (0, 0);
    }

    private void Start()
    {
        SetBoundaries();
        for (int i = 0; i < events.Count; i++)
        {
            eventToNumberMap[events[i]] = i + 1;
        }
        text = GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine(RollDicePeriodically());
    }

    private IEnumerator RollDicePeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(rollDelay);
            zombiesInScene = FindObjectsOfType<EnemyAI>().Length;
            eventBoundariesMap["Engage Zombies"] = (1, zombiesInScene);
            textAnimator.SetTrigger("spin");
            diceAnimator.SetBool("spinning", true);

            float endTime = Time.time + rollDuration;

            string ev = GetRandomEvent();
            int rand = Random.Range(eventBoundariesMap[ev].Item1, eventBoundariesMap[ev].Item2);
            while (Time.time < endTime)
            {
                ev = GetRandomEvent();
                rand = Random.Range(eventBoundariesMap[ev].Item1, eventBoundariesMap[ev].Item2);
                text.text = ev + "   " + rand.ToString();

                float delay = .2f ;
                yield return new WaitForSeconds(delay);
            }
            textAnimator.SetTrigger("done");
            diceAnimator.SetBool("spinning", false);
            SpawnerManager.instance.TriggerEvent(eventToNumberMap[ev], rand);
        }
    }

    private string GetRandomEvent()
    {
        int randomIndex = Random.Range(0, events.Count);
        if(randomIndex + 1 == eventToNumberMap["Engage Zombies"] && zombiesInScene == 0)
        {
            return GetRandomEvent();
        }
        return events[randomIndex];
    }
}
