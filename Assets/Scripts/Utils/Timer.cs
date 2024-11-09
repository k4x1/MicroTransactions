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
        Debug.Log("Timer done");
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
