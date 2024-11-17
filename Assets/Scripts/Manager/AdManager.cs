/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : AdManager.cs
/// Description : This class is responsible for managing all types of advertisements in the application.
///               It initializes the AdMob SDK and manages instances of InterstitialAdManager, RewardedAdManager, and BannerAdManager.
///               It provides methods to show different types of ads throughout the application.
/// Author : Kazuo Reis de Andrade
/// </summary>

using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AdManager Instance { get; private set; }
    private InterstitialAdManager interstitialManager;
    private RewardedAdManager rewardManager;
    private BannerAdManager bannerManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAdMob();
            interstitialManager = gameObject.AddComponent<InterstitialAdManager>();
            rewardManager = gameObject.AddComponent<RewardedAdManager>();
            bannerManager = gameObject.AddComponent<BannerAdManager>();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void InitializeAdMob()
    {
        AdMobInitializer adMobInitializer = gameObject.AddComponent<AdMobInitializer>();

        adMobInitializer.InitializeAdMob();
    }
    public void ShowInterstitial()
    {
        if (!CurrencySystem.Instance.GetAdsEnabled()) return;
        interstitialManager.ShowInterstitialAd();
    }
    public void ShowBanner()
    {
        if (!bannerManager.m_loaded) { 
            bannerManager.LoadBannerAd();
        }
        if (!CurrencySystem.Instance.GetAdsEnabled()) return;
        bannerManager.ShowBannerAd();
    }   
    public void HideBanner()
    {
        if (!CurrencySystem.Instance.GetAdsEnabled()) return;

        bannerManager.HideBannerAd();
    }
    public void ShowReward()
    {
      //  if (!CurrencySystem.Instance.GetAdsEnabled()) return;

        rewardManager.ShowRewardedAd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
