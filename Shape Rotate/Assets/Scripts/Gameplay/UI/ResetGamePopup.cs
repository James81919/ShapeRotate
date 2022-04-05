using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGamePopup : Popup
{
    public LevelManager levelManager;

    public void ResetAllProgress()
    {
        // Reset coin amount
        CoinManager.SetCoinAmount(0);

        // Reset levelsCompleted
        PuzzleLoader.DeletePackData();
        levelManager.GeneratePackData();

        // Close popup
        ClosePopup();
    }
}
