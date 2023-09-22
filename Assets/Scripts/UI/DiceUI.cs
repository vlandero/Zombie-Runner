using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceUI : MonoBehaviour
{
    private List<string> events = new List<string>(new string[] { "Event1", "Event2", "Event3", "Event4", "Event5", "Event6" });
    private TextMeshProUGUI text;
    [SerializeField] private Animator textAnimator;
    [SerializeField] private Animator diceAnimator;
    [SerializeField] private float rollDuration = 5f;
    [SerializeField] private float rollDelay = 20f;

    private void Start()
    {
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
            textAnimator.SetTrigger("done");
            diceAnimator.SetBool("spinning", false);
        }
    }

    private string GetRandomEvent()
    {
        int randomIndex = Random.Range(0, events.Count);
        return events[randomIndex];
    }
}
