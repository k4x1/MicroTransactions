using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timerDuration = 5f;
    [SerializeField] private TMP_Text timerText;

    private float timer;
    private bool timerRunning = false;

    private void Start()
    {
        StartTimer();
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
    }
}
