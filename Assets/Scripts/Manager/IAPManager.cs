/// <summary>
/// Bachelor of Software Engineering
/// Media Design School
/// Auckland
/// New Zealand
/// (c) 2024 Media Design School
/// File Name : IAPManager.cs
/// Description : This class is responsible for managing in-app purchases in the application.
///               It initializes Unity Purchasing, handles product definitions, and processes transactions
///               for gem packs and ad removal features.
/// Author : Kazuo Reis de Andrade
/// </summary>
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    public static IAPManager Instance { get; private set; }

    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;
    public bool initialized = false;
    private const string Product10000Gems = "10000_gems_product";
    private const string Product1000Gems = "1000_gems_product";
    private const string ProductNoAds = "no_ads_product";

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

    private void Start()
    {
        StartCoroutine(InitializePurchasing());
    }

    private IEnumerator InitializePurchasing()
    {
        yield return new WaitUntil(() => UnityServices.State == ServicesInitializationState.Initialized);

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(Product10000Gems, ProductType.Consumable);
        builder.AddProduct(Product1000Gems, ProductType.Consumable);
        builder.AddProduct(ProductNoAds, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
        initialized = true;
        Debug.Log("IAP Initialized successfully.");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        initialized = false;    
        Debug.LogError($"IAP Initialization failed: {error}, {message}");
    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        initialized = false;    
        Debug.LogError($"IAP Initialization failed: {error}");
    }
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        Debug.Log($"Purchased: {product.definition.id}");

        if (product.definition.id == Product10000Gems)
        {
            Debug.Log("Successfully bought 10,000 gems");
            CurrencySystem.Instance.AddCurrency(10000);
  
        }
        else if (product.definition.id == Product1000Gems)
        {
            Debug.Log("Successfully bought 1,000 gems");
            CurrencySystem.Instance.AddCurrency(1000);
            // Update user state with gems
        }
        else if (product.definition.id == ProductNoAds)
        {
            Debug.Log("Successfully removed ads");
            CurrencySystem.Instance.DisableAds();
            // Update user state to remove ads
        }
        return PurchaseProcessingResult.Complete;


    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase failed: {product.definition.id}, {failureReason}");
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogError($"Purchase failed: {product.definition.id}, {failureDescription}");
    }
    public void PurchaseItem(string productId)
    {
        if (storeController != null && storeController.products.WithID(productId) != null)
        {
            storeController.InitiatePurchase(productId);
        }
        else
        {
            Debug.LogError("Purchase failed: Product not found or IAP not initialized.");
        }
    }
}
