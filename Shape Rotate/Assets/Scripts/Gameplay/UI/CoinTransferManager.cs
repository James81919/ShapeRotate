using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoinTransferManager : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float fadeTime;

    [Header("UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        canvas.enabled = false;
    }

    public bool SpendCoins(int _amount)
    {
        int startAmount = CoinManager.GetCoinAmount();
        SetCoinAmountText(startAmount);
        if (CoinManager.SpendCoins(_amount))
        {
            coinText.text = startAmount + "<sprite=0>";

            FadeIn(() =>
            {
                AudioManager.instance.PlaySFX("UI_CoinsTransferred");
                UpdateCoinAmountText(startAmount, CoinManager.GetCoinAmount(), () =>
                {
                    FadeOut();
                });
            });


            return true;
        }

        return false;

    }
    public void AddCoins(int _amount)
    {
        int startAmount = CoinManager.GetCoinAmount();
        SetCoinAmountText(startAmount);
        CoinManager.AddCoins(_amount);

        FadeIn(() =>
        {
            AudioManager.instance.PlaySFX("UI_CoinsTransferred");
            UpdateCoinAmountText(startAmount, CoinManager.GetCoinAmount(), () =>
            {
                FadeOut();
            });
        });
    }

    private void FadeIn(Action _onComplete)
    {
        canvas.enabled = true;
        canvasGroup.alpha = 0;

        LeanTween.alphaCanvas(canvasGroup, 1, fadeTime).setOnComplete(() =>
        {
            _onComplete();
        });
    }
    private void FadeOut()
    {
        LeanTween.alphaCanvas(canvasGroup, 0, fadeTime).setDelay(1).setOnComplete(() =>
        {
            canvas.enabled = false;
        });
    }
    private void UpdateCoinAmountText(int _startAmount, int _endAmount, Action _onComplete)
    {
        LeanTween.value(_startAmount, _endAmount, duration)
            .setOnUpdate((float val) => SetCoinAmountText((int)val))
            .setOnComplete(() => {
                _onComplete();
            });
    }
    private void SetCoinAmountText(int _amount)
    {
        coinText.text = string.Format("{0:#,###0}", _amount) + "<sprite=0>";
    }
}
