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
