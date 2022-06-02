using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class BuyHintsPopup : Popup
{
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private Button watchAdButton;

    [Header("Video Ad")]
    [Tooltip("The amount of time it takes before another ad is able to be watched (in minutes)")]
    [SerializeField] private double videoAdDelay;

    private Timer rewardedVideoAdTimer;

    // IAP item IDs
    private string buy5HintsID = "com.venturegames.rotateit.5hints";
    private string buy20HintsID = "com.venturegames.rotateit.20hints";
    private string buy100HintsID = "com.venturegames.rotateit.100hints";

    public override void OnStart()
    {
        base.OnStart();
        rewardedVideoAdTimer = new Timer("rewardedVideoAdDelay", System.TimeSpan.FromMinutes(videoAdDelay));
    }

    public override void OpenPopup()
    {
        watchAdButton.interactable = AdMediationManager.Instance.CanWatchRewardedVideoAd() && rewardedVideoAdTimer.HasTimerEnded();
        base.OpenPopup();
    }

    // UI Buttons
    public void BuyHints(int _hintAmount)
    {
        HintManager.AddHints(_hintAmount);
        levelManager.UpdateHintCounter();
    }

    public void Button_WatchAdForHint()
    {
        if (AdMediationManager.Instance.CanWatchRewardedVideoAd() && rewardedVideoAdTimer.HasTimerEnded())
        {
            AdMediationManager.Instance.ShowRewardedVideoAd(() =>
            {
                ClosePopup();
                HintManager.AddHints(1);
                levelManager.UseHint();
                rewardedVideoAdTimer.StartTimer();
            });
        }
    }


    #region IAP Functions
    public void OnPurchaseComplete(Product _product)
    {
        if (_product.definition.id == buy5HintsID)
        {
            BuyHints(5);
        }
        else if (_product.definition.id == buy20HintsID)
        {
            BuyHints(20);
        }
        else if (_product.definition.id == buy100HintsID)
        {
            BuyHints(100);
        }
    }

    public void OnPurchaseFailed(Product _product, PurchaseFailureReason _reason)
    {
        Debug.Log("Purchase of " + _product.definition.id + " failed due to " + _reason);
    }
    #endregion
}
