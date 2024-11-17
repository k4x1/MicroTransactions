/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : Timer.cs
/// Description : This class implements a countdown timer functionality.
///               It manages the timer duration, updates the display,
///               and provides methods for starting, stopping, and restarting the timer.
/// Author : Kazuo Reis de Andrade
/// </summary>
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timerDuration = 5f;
    [SerializeField] private TMP_Text timerText;
    private System.Action onTimerComplete;

    private float timer;
    private bool timerRunning = false;

    public void SetOnTimerComplete(System.Action callback)
    {
        onTimerComplete = callback;
    }

    private void Start()
    {
        StopTimer();
    }

    private void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerDisplay();

            if (timer >= timerDuration)
            {
                timerRunning = false;
                TimerDone();
            }
        }
    }

    public void StartTimer()
    {
        timer = 0f;
        timerRunning = true;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Min(timerDuration, timer).ToString("F2");
        }
    }

    private void TimerDone()
    {

        if (onTimerComplete != null)
        {
            onTimerComplete.Invoke();
        }
    }

    public bool IsTimerRunning()
    {
        return timerRunning;
    }

    public float GetTimerValue()
    {
        return timer;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void RestartTimer()
    {
        StopTimer();
        StartTimer();
    }
}
