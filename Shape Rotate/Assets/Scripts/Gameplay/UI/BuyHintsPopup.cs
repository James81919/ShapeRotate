using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyHintsPopup : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private Button watchAdButton;

    [Header("Video Ad")]
    [Tooltip("The amount of time it takes before another ad is able to be watched (in minutes)")]
    [SerializeField] private double videoAdDelay;

    private Timer rewardedVideoAdTimer;

    private void Start()
    {
        popupCanvas.enabled = false;

        rewardedVideoAdTimer = new Timer("rewardedVideoAdDelay", System.TimeSpan.FromMinutes(videoAdDelay));
    }

    public void Open()
    {
        watchAdButton.interactable = AdMediationManager.Instance.CanWatchRewardedVideoAd() && rewardedVideoAdTimer.HasTimerEnded();

        popupCanvas.enabled = true;
    }

    public void Close()
    {
        popupCanvas.enabled = false;
    }


    // UI Buttons
    public void Button_BuyHints_5()
    {
        HintManager.AddHints(5);
        levelManager.UpdateHintCounter();
    }

    public void Button_BuyHints_20()
    {
        HintManager.AddHints(20);
        levelManager.UpdateHintCounter();
    }

    public void Button_BuyHints_100()
    {
        HintManager.AddHints(100);
        levelManager.UpdateHintCounter();
    }

    public void Button_WatchAdForHint()
    {
        if (AdMediationManager.Instance.CanWatchRewardedVideoAd() && rewardedVideoAdTimer.HasTimerEnded())
        {
            AdMediationManager.Instance.ShowRewardedVideoAd(() =>
            {
                Close();
                HintManager.AddHints(1);
                levelManager.UseHint();
                rewardedVideoAdTimer.StartTimer();
            });
        }
    }
}
