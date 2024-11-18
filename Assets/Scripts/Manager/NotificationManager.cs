using UnityEngine;
using Unity.Notifications.Android;
using System.Collections.Generic;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }

    private Dictionary<string, AndroidNotificationChannel> channels = new Dictionary<string, AndroidNotificationChannel>();

    private const string NotificationPermission = "android.permission.POST_NOTIFICATIONS";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize notifications and request permission
            InitializeNotifications();
            RequestNotificationPermission();
        }
        else
        {
            Destroy(gameObject);
        }
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
        System.DateTime fireTime = System.DateTime.Now.AddSeconds(1); // Fire in 5 seconds
        Debug.Log("Scheduling test notification");

        if (IsPermissionGranted())
        {
            ScheduleNotification(title, text, fireTime);
        }
        else
        {
            Debug.LogWarning("Notification permission not granted. Cannot schedule notification.");
        }
    }

    public void ScheduleDailyGemNotification()
    {
        string title = "Daily Gems Available!";
        string text = "Your daily gems are ready to be claimed!";
        System.DateTime fireTime = System.DateTime.Now.AddDays(1).Date.AddHours(8); // Set to 8 AM next day

        if (IsPermissionGranted())
        {
            ScheduleNotification(title, text, fireTime);
        }
        else
        {
            Debug.LogWarning("Notification permission not granted. Cannot schedule notification.");
        }
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

    // Notification Permission Methods

    private void RequestNotificationPermission()
    {
        // Check if the device is running Android 13 or higher
        if (IsAndroid13OrHigher())
        {
            // Check if permission is already granted
            if (!IsPermissionGranted())
            {
                // Request the notification permission
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        activity.Call("requestPermissions", new string[] { NotificationPermission }, 0);
                    }
                }
            }
            else
            {
                Debug.Log("Notification permission already granted.");
            }
        }
        else
        {
            Debug.Log("Android version is below 13; notification permission not required.");
        }
    }

    private bool IsAndroid13OrHigher()
    {
        // Get the SDK version
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            int sdkInt = version.GetStatic<int>("SDK_INT");
            return sdkInt >= 33; // Android 13 (API level 33)
        }
    }

    private bool IsPermissionGranted()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (var contextCompat = new AndroidJavaClass("androidx.core.content.ContextCompat"))
                {
                    int permissionStatus = contextCompat.CallStatic<int>("checkSelfPermission", currentActivity, NotificationPermission);
                    return permissionStatus == 0; // PackageManager.PERMISSION_GRANTED == 0
                }
            }
        }
    }
}
