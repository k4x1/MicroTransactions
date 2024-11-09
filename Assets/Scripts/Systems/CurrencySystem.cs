using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    private int currentCurrency = 0;
    private const string SAVE_FILE = "/currency.dat";
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
    }

    void OnApplicationQuit()
    {
        SaveCurrency();
    }
    private void Update()
    {
        
    }
    public void AddCurrency(int amount)
    {
        
        currentCurrency += amount;
    }  
    public int GetCurrency()
    {
        return currentCurrency;
    }
    void LoadCurrency()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE);
        if (File.Exists(filePath))
        {
            Debug.Log("currency started");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            currentCurrency = (int)formatter.Deserialize(file);
            Debug.Log(currentCurrency);
            file.Close();
        }
        else
        {
            currentCurrency = 10; 
        }
    }

    void SaveCurrency()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, SAVE_FILE));
        formatter.Serialize(file, currentCurrency);
        file.Close();
    }

    
}
