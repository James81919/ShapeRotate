using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuyPackPopup : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    [Header("Manager References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameManager gameManager;

    private int packID, packCost;

    private void Start()
    {
        popupCanvas.enabled = false;
    }

    public void Open(int _packID)
    {
        packID = _packID;
        packCost = levelManager.puzzlePacks[packID].unlockCost;

        popupCanvas.enabled = true;

        costText.text = packCost + "<sprite=0>";

        buyButton.interactable = CoinManager.GetCoinAmount() >= packCost;
    }

    public void Close()
    {
        popupCanvas.enabled = false;
    }

    // UI Buttons
    public void Button_Buy()
    {
        if (CoinManager.SpendCoins(packCost))
        {
            PuzzleLoader.SetPackUnlocked(packID, true);
            Close();
            gameManager.UpdatePackScreen();
        }
    }

    public void Button_Cancel()
    {
        Close();
    }
}
