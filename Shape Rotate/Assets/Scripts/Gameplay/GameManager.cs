using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Level Button Layout")]
    public float levelButton_startOffsetY = 500;
    public int levelButton_buttonRowSize = 4;
    public float levelButton_buttonOffset = 160;

    [Header("Pack Button Layout")]
    public float packButton_startOffsetY = -200;
    public float packButton_buttonOffset = 300;

    [Header("UI")]
    public GameObject menuScreen;
    public GameObject packSelectionScreen;
    public GameObject puzzleSelectionScreen;
    public GameObject puzzleScreen;

    [Header("UI - Pack Select")]
    public RectTransform buttonContent_packSelect;
    public TextMeshProUGUI coinAmountText;
    public BuyPackPopup buyPackPopup;

    [Header("UI - Puzzle Select")]
    public TextMeshProUGUI puzzleSelectTitleText;
    public RectTransform buttonContent_puzzleSelect;

    [Header("Prefabs")]
    public GameObject packButtonPrefab;
    public GameObject levelButtonPrefab;

    private LevelManager levelManager;

    private List<GameObject> levelButtons = new List<GameObject>();
    private List<GameObject> packButtons = new List<GameObject>();

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        levelManager.GeneratePackData();

        OpenMenuScreen();
    }

    public void OpenMenuScreen()
    {
        menuScreen.SetActive(true);
        packSelectionScreen.SetActive(false);
        puzzleSelectionScreen.SetActive(false);
        puzzleScreen.SetActive(false);
    }

    public void OpenPackScreen()
    {
        menuScreen.SetActive(false);
        packSelectionScreen.SetActive(true);
        puzzleSelectionScreen.SetActive(false);
        puzzleScreen.SetActive(false);

        UpdatePackScreen();
    }
    public void UpdatePackScreen()
    {
        // Destroy current pack buttons
        DestroyPackButtons();

        // Generate pack buttons
        GeneratePackButtons();

        coinAmountText.text = CoinManager.GetCoinAmount() + "<sprite=0>";
    }

    public void OpenLevelScreen()
    {
        OpenLevelScreen(levelManager.packID);
    }

    public void OpenLevelScreen(int _packID)
    {
        if (levelManager.puzzlePacks.Count <= _packID)
            return;

        // Destroy current level buttons
        DestroyLevelButtons();
        
        puzzleSelectTitleText.text = levelManager.puzzlePacks[_packID].packName;

        menuScreen.SetActive(false);
        packSelectionScreen.SetActive(false);
        puzzleSelectionScreen.SetActive(true);
        puzzleScreen.SetActive(false);

        // Generate level buttons
        GenerateLevelButtons(_packID);
    }

    public void LoadPuzzle(int _packID, int _levelID)
    {
        menuScreen.SetActive(false);
        packSelectionScreen.SetActive(false);
        puzzleSelectionScreen.SetActive(false);
        puzzleScreen.SetActive(true);
        levelManager.CreatePuzzle(_packID, _levelID);
    }

    private void GeneratePackButtons()
    {
        buttonContent_packSelect.sizeDelta = new Vector2(buttonContent_packSelect.sizeDelta.x, 100);

        float y = packButton_startOffsetY;
        for (int i = 0; i < levelManager.puzzlePacks.Count; i++)
        {
            buttonContent_packSelect.sizeDelta += new Vector2(0, packButton_buttonOffset);

            PackButton newButton = Instantiate(packButtonPrefab, buttonContent_packSelect).GetComponent<PackButton>();
            newButton.SetupButton(this, levelManager, buyPackPopup, i);
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
            newButton.gameObject.name = "PackButton_" + levelManager.puzzlePacks[i].packName;
            y -= packButton_buttonOffset;

            packButtons.Add(newButton.gameObject);
        }
    }

    private void DestroyPackButtons()
    {
        for (int i = 0; i < packButtons.Count; i++)
        {
            Destroy(packButtons[i]);
        }

        packButtons.Clear();
    }

    private void GenerateLevelButtons(int _packID)
    {
        buttonContent_puzzleSelect.sizeDelta = new Vector2(buttonContent_puzzleSelect.sizeDelta.x, levelButton_buttonOffset + 75);

        float x = -((levelButton_buttonRowSize - 1) * levelButton_buttonOffset) / 2;
        float y = levelButton_startOffsetY;
        for (int i = 0; i < levelManager.puzzlePacks[_packID].puzzles.Count; i++)
        {
            bool isLevelUnlocked = i <= PuzzleLoader.LoadPuzzlePackSaveData(_packID).currentLevel;

            LevelButton newButton = Instantiate(levelButtonPrefab, buttonContent_puzzleSelect).GetComponent<LevelButton>();
            newButton.SetupButton(this, _packID, i, isLevelUnlocked);
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
            if (i % levelButton_buttonRowSize == levelButton_buttonRowSize - 1)
            {
                x = -((levelButton_buttonRowSize - 1) * levelButton_buttonOffset) / 2;
                y -= levelButton_buttonOffset;
                
                buttonContent_puzzleSelect.sizeDelta += new Vector2(0, levelButton_buttonOffset);
            }
            else
                x += levelButton_buttonOffset;

            levelButtons.Add(newButton.gameObject);
        }
    }

    private void DestroyLevelButtons()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            Destroy(levelButtons[i]);
        }

        levelButtons.Clear();
    }
}
