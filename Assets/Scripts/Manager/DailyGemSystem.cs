using UnityEngine;
using System;

public class DailyGemSystem : MonoBehaviour
{
    public static DailyGemSystem Instance { get; private set; }

    [SerializeField] private int dailyGems = 100;
    [SerializeField] private string dailyGemKey = "DailyGems";

    private DateTime lastClaimTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadLastClaimTime();
    }

    public bool IsDailyGemsAvailable()
    {
        DateTime now = DateTime.Now;
        return (now - lastClaimTime).Days >= 1;
    }

    public void ClaimDailyGems()
    {
        if (IsDailyGemsAvailable())
        {
            CurrencySystem.Instance.AddCurrency(dailyGems);
            lastClaimTime = DateTime.Now;
            SaveLastClaimTime();
            Debug.Log($"Claimed {dailyGems} daily gems.");

            // Schedule next day's notification
            NotificationManager.Instance.ScheduleDailyGemNotification();
        }
        else
        {
            Debug.Log("Daily gems are not available yet.");
        }
    }

    private void LoadLastClaimTime()
    {
        if (PlayerPrefs.HasKey(dailyGemKey))
        {
            string dateString = PlayerPrefs.GetString(dailyGemKey);
            lastClaimTime = DateTime.Parse(dateString);
        }
        else
        {
            lastClaimTime = DateTime.Now.AddDays(-1); // Set to yesterday to allow immediate claim
        }
    }

    private void SaveLastClaimTime()
    {
        PlayerPrefs.SetString(dailyGemKey, lastClaimTime.ToString());
        PlayerPrefs.Save();
    }
}
