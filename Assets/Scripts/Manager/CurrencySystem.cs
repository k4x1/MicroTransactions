using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

[System.Serializable]
public class CurrencyData
{
    public int currentCurrency;
}

public class CurrencySystem : MonoBehaviour
{
    private int currentCurrency = 0;
    private const string SAVE_FILE = "/currency.json";
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
    }

    void OnApplicationQuit()
    {
        SaveCurrency();
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
            string jsonData = File.ReadAllText(filePath);
            CurrencyData data = JsonConvert.DeserializeObject<CurrencyData>(jsonData);
            currentCurrency = data.currentCurrency;
            Debug.Log($"Loaded currency: {currentCurrency}");
        }
        else
        {
            currentCurrency = 10;
            Debug.Log("No save file found. Starting with default currency.");
        }
    }

    public void SaveCurrency()
    {
        CurrencyData data = new CurrencyData { currentCurrency = currentCurrency };
        string jsonData = JsonConvert.SerializeObject(data);
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE);
        File.WriteAllText(filePath, jsonData);
        Debug.Log($"Saved currency: {currentCurrency}");
    }
}
