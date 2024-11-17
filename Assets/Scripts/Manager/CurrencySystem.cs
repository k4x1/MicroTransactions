using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class CurrencyData
{
    public int currentCurrency;
    public bool adsEnabled;
}

public class CurrencySystem : MonoBehaviour
{
    private int currentCurrency = 0;
    private const string SAVE_FILE = "/currency.dat";
    public bool adsEnabled = true;
    public static CurrencySystem Instance { get; private set; }

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
    }

    void Start()
    {
        LoadCurrency();
        if (MainMenuUi.Instance != null)
        {
            MainMenuUi.Instance.UpdateGems();
        }
        Debug.Log(SAVE_FILE);
    }

    void OnApplicationQuit()
    {
        SaveCurrency();
    }

    public bool GetAdsEnabled()
    {
        return adsEnabled;
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        if (MainMenuUi.Instance != null)
        {
            MainMenuUi.Instance.UpdateGems();
        }
        if (UiManager.Instance != null)
        {
            UiManager.Instance.UpdateGems(currentCurrency);
        }
        SaveCurrency();
        if (currentCurrency > 200)
        {
            GameManager.Instance.UnlockAchievement(GPGSIds.achievement_gems_a_plenty);
        }
    }

    public int GetCurrency()
    {
        return currentCurrency;
    }

    public void LoadCurrency()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE);
        if (File.Exists(filePath))
        {
            byte[] data = File.ReadAllBytes(filePath);
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);
            CurrencyData loadedData = formatter.Deserialize(stream) as CurrencyData;
            currentCurrency = loadedData.currentCurrency;
            adsEnabled = loadedData.adsEnabled;
            Debug.Log($"Loaded currency: {currentCurrency}, Ads Enabled: {adsEnabled}");
        }
        else
        {
            currentCurrency = 10;
            adsEnabled = true;
            Debug.Log("No save file found. Starting with default currency and ads enabled.");
        }
    }

    public void SaveCurrency()
    {
        CurrencyData data = new CurrencyData { currentCurrency = currentCurrency, adsEnabled = adsEnabled };
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize(stream, data);
        byte[] bytes = stream.ToArray();
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE);
        File.WriteAllBytes(filePath, bytes);
        Debug.Log($"Saved currency: {currentCurrency}, Ads Enabled: {adsEnabled}");
    }

    public void DisableAds()
    {
        adsEnabled = false;
        SaveCurrency();
        Debug.Log("Ads have been disabled.");
    }
}
