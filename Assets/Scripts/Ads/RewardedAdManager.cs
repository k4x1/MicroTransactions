/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : RewardedAdManager.cs
/// Description : This class is responsible for managing rewarded advertisements in the application.
///               It loads and displays full-screen rewarded ads using the Google Mobile Ads SDK,
///               providing rewards to users upon successful completion of the ad.
/// Author : Kazuo Reis de Andrade
/// </summary>
using UnityEngine;
using GoogleMobileAds.Api;

public class RewardedAdManager : MonoBehaviour
{
    private RewardedAd m_rewardedAd;
    [SerializeField] private string m_adUnitID = "ca-app-pub-4010580083693927/3000500257";// "ca-app-pub-3940256099942544/5224354917";
    private const int REWARD_AMOUNT = 100;
    void Start()
    {
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        if (m_rewardedAd != null)
        {
            m_rewardedAd.Destroy();
            m_rewardedAd = null;
        }

        Debug.Log("Loading rewarded ad...");

        AdRequest adRequest = new AdRequest();
        RewardedAd.Load(m_adUnitID, adRequest, (RewardedAd ad, LoadAdError err) =>
        {
            if (err != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + err);
                return;
            }

            Debug.Log("Rewarded ad loaded with response: " + ad.GetResponseInfo());
            m_rewardedAd = ad;

            // Add event handlers
            AdEventHandlers(m_rewardedAd);
        });
    }

    void AdEventHandlers(RewardedAd _ad)
    {
        _ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad clicked");
        };

        _ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad opened");
        };

        _ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad closed");
            LoadRewardedAd(); // Reload the ad
        };

        _ad.OnAdFullScreenContentFailed += (AdError _err) =>
        {
            Debug.LogError("Rewarded ad failed, error: " + _err);
            LoadRewardedAd(); // Reload the ad
        };
    }

    public bool ShowRewardedAd()
    {
        if (m_rewardedAd != null && m_rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad");
            m_rewardedAd.Show(OnUserEarnedReward);
            return true;
        }
        else
        {
            Debug.Log("Showing rewarded ad failed. Ad not ready");
            return false;
        }
    }

    private void OnUserEarnedReward(Reward reward)
    {
        Debug.Log("User earned reward: " + reward.Type + ", amount: " + reward.Amount);

        // Add gems to the player's currency
        if (CurrencySystem.Instance != null)
        {
            CurrencySystem.Instance.AddCurrency(REWARD_AMOUNT);
            Debug.Log($"Added {REWARD_AMOUNT} gems to player's currency.");
        }
        else
        {
            Debug.LogWarning("CurrencySystem instance not found. Unable to add reward.");
        }
    }

    void OnDestroy()
    {
        if (m_rewardedAd != null)
        {
            m_rewardedAd.Destroy();
            m_rewardedAd = null;
        }
    }
}