
/// <summary>
///Bachelor of Software Engineering
///Media Design School
///Auckland
///New Zealand
///(c) 2024 Media Design School
///File Name : AdMobInitializer.cs
///Description : This class is responsible for initializing the Google Mobile Ads SDK. 
///              It ensures that the SDK is ready to load and display ads within the application.
///Author : Kazuo Reis de Andrade
///
/// </summary>
using UnityEngine;
using GoogleMobileAds.Api;
public class AdMobInitializer : MonoBehaviour
{
    // Start is called before the first frame update
 
    public void InitializeAdMob()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => {
            Debug.Log("AdMob SDK initialized.");
            foreach (var adapterStatus in initStatus.getAdapterStatusMap())
            {
                Debug.Log("Adapter: " + adapterStatus.Key + " Status: " + adapterStatus.Value.InitializationState);
            }
        });
    }
}
