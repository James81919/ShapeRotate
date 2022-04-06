using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateGamePopup : Popup
{
    public int levelsCompleteBeforePopup = 10;

    [Header("IDs")]
    public string googlePlayBundleID;
    public string iosAppID;

    public void CheckCanAppear()
    {
        if (!IsRated())
        {
            if (GetCountToNextPopup() <= 0)
            {
                SetCountToNextPopup(levelsCompleteBeforePopup);
                OpenPopup();
            }
            else
                SetCountToNextPopup(GetCountToNextPopup() - 1);
        }
    }

    private bool IsRated()
    {
        return PlayerPrefs.GetInt("hasRatedGame", 0) > 0;
    }
    private void SetIsRated(bool _b)
    {
        PlayerPrefs.SetInt("hasRatedGame", _b ? 1 : 0);
    }

    private int GetCountToNextPopup()
    {
        return PlayerPrefs.GetInt("levelsRemainingBeforeRatePopup", levelsCompleteBeforePopup);
    }
    private void SetCountToNextPopup(int _count)
    {
        PlayerPrefs.SetInt("levelsRemainingBeforeRatePopup", _count);
    }

    private void Rate()
    {
#if UNITY_ANDROID

#if UNITY_EDITOR
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + googlePlayBundleID);
#else
        Application.OpenURL("market://details?id=" + googlePlayBundleID);
#endif
#endif
#if UNITY_IOS
            Application.OpenURL("https://itunes.apple.com/app/id" + iosAppID);
#endif

        SetIsRated(true);
        ClosePopup();
    }

    // UI buttons
    public void Button_Yes()
    {
        Rate();
    }

    public void Button_Later()
    {
        // Set popup to appear later


        // Close the popup
        ClosePopup();
    }
}
