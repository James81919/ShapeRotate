using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class AdMediationManager : MonoBehaviour, IRewardedVideoAdListener
{
    public static AdMediationManager Instance { get; private set; }

    public delegate void AdRewardFunc();
    AdRewardFunc adRewardFunc;

    public bool isTesting;
    [Tooltip("Ad delay in minutes")]
    public float adMinTimeDelay;

    private bool isAdReady;

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

            isAdReady = true;

            InitialiseAds();
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
        Appodeal.initialize(appKey_Android, Appodeal.REWARDED_VIDEO);
#elif UNITY_IPHONE
        Appodeal.initialize(appKey_iOS, Appodeal.REWARDED_VIDEO);
#endif
        Appodeal.setRewardedVideoCallbacks(this);
    }

    public void EnableAdReady()
    {
        isAdReady = true;
    }

    public void DestroyAd()
    {
        Appodeal.destroy(Appodeal.REWARDED_VIDEO);
    }

    public bool CanWatchAd()
    {
        if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO) && isAdReady)
            return true;

        return false;
    }

    public void ShowRewardBasedAd(AdRewardFunc _onAdReward)
    {
        adRewardFunc = _onAdReward;

        if (CanWatchAd())
            Appodeal.show(Appodeal.REWARDED_VIDEO);
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
            isAdReady = false;
            Invoke("EnableAdReady", adMinTimeDelay * 60);
        }
    }

    public void onRewardedVideoExpired()
    {

    }

    public void onRewardedVideoClicked()
    {

    }
#endregion
}
