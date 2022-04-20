using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGamePopup : Popup
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private SettingsMenu settingsMenu;

    public void ResetAllProgress()
    {
        // Reset coin amount
        CoinManager.SetCoinAmount(0);

        // Reset levelsCompleted
        PuzzleLoader.DeletePackData();
        levelManager.GeneratePackData();
        ColourPaletteManager.SetSelectedColourPalette(0);

        // Close popup
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
