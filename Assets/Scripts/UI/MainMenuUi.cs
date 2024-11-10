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
