using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdMediationManager : MonoBehaviour, IRewardedVideoAdListener, IInterstitialAdListener
{
    public static AdMediationManager Instance { get; private set; }

    public delegate void AdRewardFunc();
    AdRewardFunc adRewardFunc;

    public bool isTesting;

    [Tooltip("The delay between each interstitial ad (in seconds)")]
    public float interstitialAdDelay;

    private bool isInterstitialAdReady;

#if UNITY_ANDROID
    private string appKey_Android = "6633505ef46ecb5855b43c6184b0c56ba6e463ce49fdd9bd";
#elif UNITY_IPHONE
    private string appKey_iOS = "3202e6347b14795f82c406348731e7f716d0e3c65dccc0be";
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitialiseAds();

            DisableInterstititalAdReady();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitialiseAds()
    {
        Appodeal.disableLocationPermissionCheck();
        Appodeal.setTesting(isTesting);
#if UNITY_ANDROID
        Appodeal.initialize(appKey_Android, Appodeal.REWARDED_VIDEO | Appodeal.INTERSTITIAL);
#elif UNITY_IPHONE
        Appodeal.initialize(appKey_iOS, Appodeal.REWARDED_VIDEO);
#endif
        Appodeal.setRewardedVideoCallbacks(this);
    }

    public void DestroyAd()
    {
        Appodeal.destroy(Appodeal.REWARDED_VIDEO | Appodeal.INTERSTITIAL);
    }
    
    // Rewarded Video Ad Functions
    public bool CanWatchRewardedVideoAd()
    {
        return Appodeal.isLoaded(Appodeal.REWARDED_VIDEO);
    }

    public void ShowRewardedVideoAd(AdRewardFunc _onAdReward)
    {
        adRewardFunc = _onAdReward;

        if (CanWatchRewardedVideoAd())
            Appodeal.show(Appodeal.REWARDED_VIDEO);
    }

    // Interstitial Ad Functions
    public void EnableInterstitialAdReady()
    {
        isInterstitialAdReady = true;
    }

    public void DisableInterstititalAdReady()
    {
        isInterstitialAdReady = false;
        Invoke("EnableInterstitialAdReady", interstitialAdDelay * 60);
    }

    public bool CanInterstitialAdAppear()
    {
        return Appodeal.isLoaded(Appodeal.INTERSTITIAL) && isInterstitialAdReady;
    }

    public void ShowInterstitialAd()
    {
        if (CanInterstitialAdAppear())
        {
            Appodeal.show(Appodeal.INTERSTITIAL);
            DisableInterstititalAdReady();
        }
    }

    #region Appodeal Rewarded Video Ad Listeners
    public void onRewardedVideoLoaded(bool precache)
    {

    }

    public void onRewardedVideoFailedToLoad()
    {

    }

    public void onRewardedVideoShowFailed()
    {

    }

    public void onRewardedVideoShown()
    {

    }

    public void onRewardedVideoFinished(double amount, string name)
    {
        
    }

    public void onRewardedVideoClosed(bool finished)
    {
        if (finished)
        {
            // Reward player
            adRewardFunc();
        }
    }

    public void onRewardedVideoExpired()
    {

    }

    public void onRewardedVideoClicked()
    {

    }
    #endregion

    #region Appodeal Interstitial Ad Listeners
    public void onInterstitialLoaded(bool isPrecache)
    {
        
    }

    public void onInterstitialFailedToLoad()
    {
        
    }

    public void onInterstitialShowFailed()
    {
        
    }

    public void onInterstitialShown()
    {
        
    }

    public void onInterstitialClosed()
    {
        
    }

    public void onInterstitialClicked()
    {
        
    }

    public void onInterstitialExpired()
    {
        
    }
    #endregion
}
