/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : MainMenuUi.cs
/// Description : This class manages the main menu user interface.
///               It handles toggling between the shop and main menu screens,
///               and updates the display of the player's gem count.
/// Author : Kazuo Reis de Andrade
/// </summary>
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUi : MonoBehaviour
{
    // Start is called before the first frame update
    public static MainMenuUi Instance { get; private set; }
    [SerializeField] private GameObject Shop;
    [SerializeField] private GameObject Main;
    [SerializeField] TMP_Text Gems;

    [SerializeField] private GameObject dailyGemButton;
    [SerializeField] private TMP_Text dailyGemText;
    [SerializeField] private GameObject testNotificationButton;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            /*  DontDestroyOnLoad(gameObject);*/
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    private void Start()
    {
        UpdateGems();
        UpdateDailyGemUI();
        
    }

    public void UpdateDailyGemUI()
    {
        if (dailyGemText != null)
        {
            if (DailyGemSystem.Instance.IsDailyGemsAvailable())
            {
                dailyGemText.text = "Claim Daily Gems";
            }
            else
            {
                dailyGemText.text = "Daily Gems Unavailable";
            }
        }
        
    }
    public void TestButton()
    {
        NotificationManager.Instance.TestNotification();
    }
    public void ClaimDailyGems()
    {
        DailyGemSystem.Instance.ClaimDailyGems();
        UpdateGems();
        UpdateDailyGemUI();

    }
    public void ToggleShop()
    {
        Shop.SetActive(!Shop.activeSelf);
        Main.SetActive(!Shop.activeSelf);
    }
    public void UpdateGems()
    {
        Gems.text = "Gems: " + CurrencySystem.Instance.GetCurrency();
    }
}
