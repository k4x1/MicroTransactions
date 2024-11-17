using UnityEngine;
using Unity.Notifications.Android;
using System.Collections.Generic;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    private Dictionary<string, AndroidNotificationChannel> channels = new Dictionary<string, AndroidNotificationChannel>();

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

        InitializeNotifications();
    }

    private void InitializeNotifications()
    {
        AndroidNotificationCenter.Initialize();
        CreateNotificationChannels();
    }

    private void CreateNotificationChannels()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        channels.Add(channel.Id, channel);
    }
    public void TestNotification()
    {
        string title = "Test Notification";
        string text = "This is a test notification";
        System.DateTime fireTime = System.DateTime.Now.AddSeconds(5); // Fire in 5 seconds
        Debug.Log("test notification");
        ScheduleNotification(title, text, fireTime);
    }
    public void ScheduleDailyGemNotification()
    {
        string title = "Daily Gems Available!";
        string text = "Your daily gems are ready to be claimed!";
        System.DateTime fireTime = System.DateTime.Now.AddDays(1).AddHours(8); // Set to 8 AM next day

        ScheduleNotification(title, text, fireTime);
    }
    public void ScheduleNotification(string title, string text, System.DateTime fireTime)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = fireTime;

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }

    public void CancelAllNotifications()
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }

    public void CancelNotification(int notificationId)
    {
        AndroidNotificationCenter.CancelNotification(notificationId);
    }

    public void OnNotificationReceived(AndroidNotificationIntentData data)
    {
        Debug.Log("Received Android notification: " + data.Notification.Title);
    }

    private void OnEnable()
    {
        AndroidNotificationCenter.OnNotificationReceived += OnNotificationReceived;
    }

    private void OnDisable()
    {
        AndroidNotificationCenter.OnNotificationReceived -= OnNotificationReceived;
    }
}
