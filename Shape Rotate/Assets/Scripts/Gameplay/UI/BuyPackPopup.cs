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
    [SerializeField] private CoinTransferManager coinTransferManager;

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

        costText.text = string.Format("{0:#,###0}", packCost) + "<sprite=0>";

        buyButton.interactable = CoinManager.GetCoinAmount() >= packCost;
    }

    public void Close()
    {
        popupCanvas.enabled = false;
    }

    // UI Buttons
    public void Button_Buy()
    {
        if (coinTransferManager.SpendCoins(packCost))
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
