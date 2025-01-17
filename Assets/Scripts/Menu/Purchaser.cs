﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : Singleton<Purchaser>, IStoreListener
{
    public static IStoreController m_StoreController;          // The Unity Purchasing system.
    public static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.


    public static string COINS_299 = "gp_299";
    //string COINS_299_GP = "gp_coins_299";
    //string COINS_299_AS = "as_coins_299";


    public static string COINS_99 = "gp_99";
    //string COINS_99_GP = "gp_coins_99";
    //string COINS_99_AS = "as_coins_99";


    public static string NO_ADS = "gp_noads";
    //string NO_ADS_GP = "gp_noads";
    //string NO_ADS_AS = "as_noads";

    public static string DESTROYER = "gp_destroyer";
    //string DESTROYER_GP = "gp_destroyer";
    //string DESTROYER_AS = "as_destroyer";

    public static string FIGTER = "gp_fighter";
    //string FIGTER_GP = "gp_fighter";
    //string FIGTER_AS = "as_fighter";

    public static string UFO = "gp_ufo";
    //string UFO_GP = "gp_ufo";
    //string UFO_AS = "as_ufo";

    public static string PROMETHEUS = "gp_prometheus";
    //string PROMETHEUS_GP = "gp_prometheus";
    //string PROMETHEUS_AS = "as_prometheus";

    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Consumable
        builder.AddProduct(COINS_299, ProductType.Consumable);
        builder.AddProduct(COINS_99, ProductType.Consumable);
        // Non-consumable 
        builder.AddProduct(DESTROYER, ProductType.NonConsumable);
        builder.AddProduct(FIGTER, ProductType.NonConsumable);
        builder.AddProduct(UFO, ProductType.NonConsumable);
        builder.AddProduct(PROMETHEUS, ProductType.NonConsumable);
        builder.AddProduct(NO_ADS, ProductType.NonConsumable);

        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
    }

    public string GetPriceFromStore(string id)
    {
        if (m_StoreController != null && m_StoreExtensionProvider != null)
            return m_StoreController.products.WithID(id).metadata.localizedPriceString;
        else
            return "";
    }

    public bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyDestroyer()
    {
        BuyProductID(DESTROYER);
    }
    public void BuyFighter()
    {
        BuyProductID(FIGTER);
    }
    public void BuyUFO()
    {
        BuyProductID(UFO);
    }
    public void BuyPrometheus()
    {
        BuyProductID(PROMETHEUS);
    }
    public void BuyCoins99()
    {
        BuyProductID(COINS_99);
    }
    public void BuyCoins299()
    {
        BuyProductID(COINS_299);
    }
    public void BuyNoAds()
    {
        BuyProductID(NO_ADS);
    }

    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log($"BuyProductID: FAIL. Not purchasing product {product}, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, COINS_99, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
           
            Store.Instance.AddCoins(99);
        }
        
        else if(String.Equals(args.purchasedProduct.definition.id, COINS_299, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            
            Store.Instance.AddCoins(299);    
        }
       
        else if (String.Equals(args.purchasedProduct.definition.id, NO_ADS, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
           
            Store.Instance.NoAds();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, DESTROYER, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            //Store.Instance.UnlockDestroyer();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, FIGTER, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            Store.Instance.UnlockFighter();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, UFO, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            Store.Instance.UnlockUFO();
        }
        else if (String.Equals(args.purchasedProduct.definition.id, PROMETHEUS, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            Store.Instance.UnlockPrometheus();
        }

        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
