using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeElapsedHandler : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float startTime;
    private bool isRunning = false;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        timerText.text = "0.00s";
        StartTimer();
    }

    private void Update()
    {
        if (isRunning)
        {
            float elapsedTime = Time.time - startTime;
            UpdateTimerText(elapsedTime);
        }
    }

    public void StartTimer()
    {
        if (!isRunning)
        {
            startTime = Time.time;
            isRunning = true;
        }
    }

    public void StopTimer()
    {
        if (isRunning)
        {
            isRunning = false;
        }
    }

    public void ResetTimer()
    {
        isRunning = false;
        UpdateTimerText(0f);
    }

    private void UpdateTimerText(float timeInSeconds)
    {
        float minutes = Mathf.FloorToInt(timeInSeconds / 60);
        float seconds = timeInSeconds % 60;
        timerText.text = string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }
}
