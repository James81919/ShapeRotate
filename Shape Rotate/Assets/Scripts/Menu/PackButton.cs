using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PackButton : MonoBehaviour
{
    public TextMeshProUGUI packNameText;

    [Header("Unlocked")]
    public GameObject levelCountGO;
    public TextMeshProUGUI levelCountText;

    [Header("Locked")]
    public GameObject buyPackGO;
    public TextMeshProUGUI buyPackText;

    public void SetupButton(GameManager _gameManager, LevelManager _levelManager, int _packID)
    {
        PuzzlePack puzzlePack = _levelManager.puzzlePacks[_packID];

        packNameText.text = puzzlePack.packName;

        if (PuzzleLoader.LoadPuzzlePackSaveData(_packID).isUnlocked)
        {
            buyPackGO.SetActive(false);
            levelCountGO.SetActive(true);
            levelCountText.text = PuzzleLoader.LoadPuzzlePackSaveData(_packID).currentLevel + " / " + puzzlePack.puzzles.Count;

            GetComponent<Button>().onClick.AddListener(() => _gameManager.OpenLevelScreen(_packID));
        }
        else
        {
            buyPackGO.SetActive(true);
            levelCountGO.SetActive(false);
        }
    }
}
