using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCompletePopup : MonoBehaviour
{
    public int coinRewardAmount = 5;

    [Header("UI")]
    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI coinAmountText;
    [SerializeField] private RectTransform background;
    [SerializeField] private TextMeshProUGUI levelCompleteText;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private CanvasGroup buttonsCanvasGroup;

    [Header("Manager References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CoinTransferManager coinTransferManager;

    private int packID, levelID;

    private Color backgroundColor;

    private void Start()
    {
        popupCanvas.enabled = false;

        backgroundColor = background.GetComponent<Image>().color;
    }

    public void Appear(int _packID, int _levelID)
    {
        // Setting pack and level IDs
        packID = _packID;
        levelID = _levelID;

        // Enable canvas
        popupCanvas.enabled = true;
        // Enable canvas group
        canvasGroup.alpha = 1;
        // Set background opacity to 0
        background.GetComponent<Image>().color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
        // Set levelComplete text opacity to 0
        levelCompleteText.alpha = 0;
        // Set coin amount text opacity to 0
        coinAmountText.alpha = 0;
        // Set buttons opacity to 0
        buttonsCanvasGroup.alpha = 0;

        FadeInBackground(() =>
        {
            FadeInLevelCompleteText(() =>
            {
                if (PuzzleLoader.IsLevelUnlocked(_packID, _levelID + 1))
                {
                    coinAmountText.gameObject.SetActive(true);
                    coinAmountText.text = "+" + coinRewardAmount + "<sprite=0>";
                    LeanTween.delayedCall(0.6f, () => RewardPlayer());
                    FadeInCoinAmountText(() =>
                    {
                        FadeInButtons(0.6f);
                    }, 0.4f);
                }
                else
                {
                    coinAmountText.gameObject.SetActive(false);
                    FadeInButtons();
                }
            });
        });
    }
    public void Disappear()
    {
        homeButton.enabled = false;
        nextButton.enabled = false;

        LeanTween.alphaCanvas(canvasGroup, 0, 0.25f).setOnComplete(() => popupCanvas.enabled = false);
    }
    
    private void RewardPlayer()
    {
        coinTransferManager.AddCoins(coinRewardAmount);
    }

    // UI Animations
    private void FadeInBackground(Action _onComplete)
    {
        LeanTween.alpha(background, backgroundColor.a, 0.25f).setOnComplete(() => _onComplete());
    }
    private void FadeInLevelCompleteText(Action _onComplete, float _delay = 0)
    {
        levelCompleteText.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 1);
        LeanTween.scale(levelCompleteText.GetComponent<RectTransform>(), new Vector3(1,1,1), 0.5f).setEase(LeanTweenType.easeOutBack);
        LeanTweenExt.LeanAlphaText(levelCompleteText, 1, 0.5f)
            .setDelay(_delay)
            .setOnStart(() => AudioManager.instance.PlaySFX("LevelComplete"))
            .setOnComplete(() => _onComplete());
    }
    private void FadeInCoinAmountText(Action _onComplete, float _delay = 0)
    {
        LeanTweenExt.LeanAlphaText(coinAmountText, 1, 0.5f)
            .setDelay(_delay)
            .setOnComplete(() => { _onComplete(); });
    }
    private void FadeInButtons(float _delay = 0)
    {
        homeButton.enabled = false;
        nextButton.enabled = false;

        buttonsCanvasGroup.alpha = 0;

        LeanTween.alphaCanvas(buttonsCanvasGroup, 1, 0.4f).setDelay(_delay).setOnComplete(() =>
        {
            homeButton.enabled = true;
            nextButton.enabled = true;
        });
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
