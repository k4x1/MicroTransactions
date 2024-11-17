/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : BannerAdManager.cs
/// Description : This class is responsible for managing banner advertisements in the application.
///               It creates, loads, shows, and hides banner ads using the Google Mobile Ads SDK.
/// Author : Kazuo Reis de Andrade
/// </summary>
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEditor;

public class BannerAdManager : MonoBehaviour
{
    private BannerView m_bannerView;
    //
    [SerializeField] private string m_adUnitID = "ca-app-pub-3940256099942544/6300978111"; //     "ca-app-pub-4010580083693927~4005689652"
    public bool m_loaded = false;
    void Start()
    {
        CreateBannerView();
    }
    public void CreateBannerView()
    {
        if (m_bannerView != null)
        {
            m_bannerView.Destroy();
            m_bannerView = null;
        }

        Debug.Log("Creating banner view...");

        // Get the safe area of the screen
        Rect safeArea = Screen.safeArea;
        Debug.Log(safeArea.yMax + "|" + safeArea.yMin);

       
        int yPosition = Mathf.RoundToInt(safeArea.yMax/3 -20);

        // Create the banner view with custom position at the bottom of the safe area
      
        m_bannerView = new BannerView(m_adUnitID, AdSize.Banner,  AdPosition.Bottom);

      
    }

    public void LoadBannerAd()
    {
        if (m_loaded) return;

        if (m_bannerView == null)
        {
            CreateBannerView();
        }

        AdRequest adRequest = new AdRequest();
        Debug.Log("Loading banner ad...");
        m_bannerView.LoadAd(adRequest);
        m_bannerView.Hide();


        // Add event handlers
        AdEventHandlers(m_bannerView);
        m_loaded = true;
    }


    void AdEventHandlers(BannerView _ad)
    {
        _ad.OnAdClicked += () =>
        {
            Debug.Log("Banner ad clicked");
        };

        _ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner ad opened");
        };

        _ad.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner ad loaded");
          
        };

        _ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner ad closed");
        };

        _ad.OnBannerAdLoadFailed += (LoadAdError _err) =>
        {
            Debug.LogError("Banner ad failed, error: " + _err);
        };
    }

    public void ShowBannerAd()
    {
        if (m_bannerView != null)
        {
            Debug.Log("Showing banner ad");
            m_bannerView.Show();
        }
        else
        {
            Debug.LogWarning("Banner ad is not ready to be shown");
        }
    }

    public void HideBannerAd()
    {
        if (m_bannerView != null)
        {
            Debug.Log("Destroying banner ad");
            m_bannerView.Hide();
           // m_bannerView.Destroy();
            m_bannerView = null;
            m_loaded = false;
        }
        else
        {
            Debug.LogWarning("Banner ad is already null or not loaded");
        }
    }


    void OnDestroy()    
    {
        if (m_bannerView != null)
        {
            m_bannerView.Destroy();
            m_bannerView = null;
        }
    }
}
    