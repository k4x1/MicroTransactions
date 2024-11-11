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
        if (!GameManager.Instance.adsEnabled) return;
        interstitialManager.ShowInterstitialAd();
    }
    public void ShowBanner()
    {
        if (!bannerManager.m_loaded) { 
            bannerManager.LoadBannerAd();
        }
        if (!GameManager.Instance.adsEnabled) return;
        bannerManager.ShowBannerAd();
    }   
    public void HideBanner()
    {
        if (!GameManager.Instance.adsEnabled) return;

        bannerManager.HideBannerAd();
    }
    public void ShowReward()
    {
        if (!GameManager.Instance.adsEnabled) return;

        rewardManager.ShowRewardedAd();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
