using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceUI : MonoBehaviour
{
    private readonly List<string> events = new List<string>(new string[] { "Zombie Spawn", "Ammo Spawn", "Health Spawn" });
    private readonly Dictionary<string, int> eventToNumberMap = new();

    private TextMeshProUGUI text;
    [SerializeField] private Animator textAnimator;
    [SerializeField] private Animator diceAnimator;
    [SerializeField] private float rollDuration = 10f;
    [SerializeField] private float rollDelay = 20f;

    private void Start()
    {
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
            textAnimator.SetTrigger("spin");
            diceAnimator.SetBool("spinning", true);

            float endTime = Time.time + rollDuration;

            while (Time.time < endTime)
            {
                text.text = GetRandomEvent();

                float remainingTime = endTime - Time.time;
                float delay = Mathf.Lerp(0.1f, 1.0f, 1.0f - (remainingTime / rollDuration));
                yield return new WaitForSeconds(delay);
            }

            text.text = GetRandomEvent();
            int rand = Random.Range(1, 7);
            textAnimator.SetTrigger("done");
            diceAnimator.SetBool("spinning", false);
            SpawnerManager.instance.TriggerEvent(eventToNumberMap[text.text], rand);
        }
    }

    private string GetRandomEvent()
    {
        int randomIndex = Random.Range(0, events.Count);
        return events[randomIndex];
    }
}
