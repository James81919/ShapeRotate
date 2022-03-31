using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompletePopup : MonoBehaviour
{
    public int coinRewardAmount = 5;

    [Header("UI")]
    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private TextMeshProUGUI coinAmountText;

    [Header("Manager References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameManager gameManager;

    private int packID, levelID;

    private void Start()
    {
        popupCanvas.enabled = false;
    }

    public void Appear(int _packID, int _levelID)
    {
        packID = _packID;
        levelID = _levelID;
        popupCanvas.enabled = true;

        if (PuzzleLoader.IsLevelUnlocked(_packID, _levelID + 1))
        {
            coinAmountText.gameObject.SetActive(true);
            coinAmountText.text = "+" + coinRewardAmount + "<sprite=0>";
            RewardPlayer();
        }
        else
        {
            coinAmountText.gameObject.SetActive(false);
        }
    }
    public void Disappear()
    {
        popupCanvas.enabled = false;
    }
    
    private void RewardPlayer()
    {
        CoinManager.AddCoins(coinRewardAmount);
    }

    // UI Buttons
    private void GoToNextLevel()
    {
        levelManager.ClearPuzzle();
        levelManager.CreatePuzzle(packID, levelID + 1);

        Disappear();
    }
    public void Button_NextLevel()
    {
        if (AdMediationManager.Instance.CanInterstitialAdAppear())
        {
            AdMediationManager.Instance.ShowInterstitialAd();
            GoToNextLevel();
        }
        else
        {
            GoToNextLevel();
        }
    }
    public void Button_Home()
    {
        gameManager.OpenLevelScreen();

        Disappear();
    }
}
