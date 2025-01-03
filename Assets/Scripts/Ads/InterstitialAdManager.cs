/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : InterstitialAdManager.cs
/// Description : This class is responsible for managing interstitial advertisements in the application.
///               It loads and displays full-screen interstitial ads using the Google Mobile Ads SDK.
/// Author : Kazuo Reis de Andrade
/// </summary>
using UnityEngine;
using GoogleMobileAds.Api;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd m_interstitialAd;
    [SerializeField] private string m_adUnitID = "ca-app-pub-4010580083693927/3117991691";// "ca-app-pub-3940256099942544/1033173712";

    void Start()
    {
        LoadInterstitialAd();
      
    }

    public void LoadInterstitialAd()
    {
        if (m_interstitialAd != null)
        {
            m_interstitialAd.Destroy();
            m_interstitialAd = null;
        }

        Debug.Log("Loading interstitial ad...");

        AdRequest adRequest = new AdRequest();

        InterstitialAd.Load(m_adUnitID, adRequest, (InterstitialAd ad, LoadAdError err) =>
        {
            if (err != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + err);
                return;
            }

            Debug.Log("Interstitial ad loaded with response: " + ad.GetResponseInfo());
            m_interstitialAd = ad;

            // Add event handlers
            AdEventHandlers(m_interstitialAd);
        });
    }

    void AdEventHandlers(InterstitialAd _ad)
    {
        _ad.OnAdClicked += () =>
        {
            print("ad clicked");
        };

        _ad.OnAdFullScreenContentOpened += () =>
        {
            print("ad opened");
        };

        _ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Ad closed");
            LoadInterstitialAd(); // Reload the ad
        };

        _ad.OnAdFullScreenContentFailed += (AdError _err) =>
        {
            print("ad failed, error: " + _err);
            LoadInterstitialAd(); // Reload the ad
        };
    }

    public bool ShowInterstitialAd()
    {
        if (m_interstitialAd != null && m_interstitialAd.CanShowAd())
        {
            Debug.Log("Showing Ad");
            m_interstitialAd.Show();
            return true;
        }
        else
        {
            Debug.Log("Showing Ad failed. Ad not ready");
            return false;
        }
    }

    void OnDestroy()
    {
        if (m_interstitialAd != null)
        {
            m_interstitialAd.Destroy();
            m_interstitialAd = null;
        }
    }
}
