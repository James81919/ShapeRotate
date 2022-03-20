using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public List<ColorPalette> colorPalettes;
    public List<PuzzlePack> puzzlePacks;
    private int colorPalette = 0;

    public float splitOffset;

    [Header("Prefabs")]
    public GameObject shapeParentPrefab;
    public GameObject tilePrefab;

    [Header("UI")]
    public Transform puzzleParent;
    public GameObject nextButton;
    public TextMeshProUGUI levelNumberText;
    public GridIconManager gridIconManager;
    public TextMeshProUGUI gridSizeText;

    [HideInInspector] public bool isLevelComplete;

    private List<GameObject> tiles = new List<GameObject>();
    private List<Shape> shapes = new List<Shape>();
    private float tileSize = 75;

    private float maxPuzzleSize = 300;

    [HideInInspector] public int packID, levelID;

    public void GeneratePackData()
    {
        if (PuzzleLoader.LoadPuzzlePacks() == null)
        {
            List<PuzzlePackSaveData> packs = new List<PuzzlePackSaveData>();
            for (int i = 0; i < puzzlePacks.Count; i++)
            {
                PuzzlePackSaveData saveData;
                saveData.currentLevel = 0;
                saveData.isUnlocked = !puzzlePacks[i].isLocked;
                packs.Add(saveData);
            }
            
            PuzzleLoader.SavePuzzlePacks(packs);
        }
    }

    public void CreatePuzzle(int _packID, int _levelID)
    {
        ClearPuzzle();

        if (puzzlePacks.Count <= _packID)
            return;

        if (puzzlePacks[_packID].puzzles.Count <= _levelID)
            return;

        nextButton.SetActive(false);

        packID = _packID;
        levelID = _levelID;

        levelNumberText.text = (_levelID + 1).ToString();

        PuzzleData puzzle = puzzlePacks[_packID].puzzles[_levelID];

        // Set tile size
        tileSize = (puzzle.width > puzzle.height) ? maxPuzzleSize / puzzle.width : maxPuzzleSize / puzzle.height;

        // Update grid size UI text
        gridSizeText.text = puzzle.width + " x " + puzzle.height;

        CreatePuzzleGrid(puzzle);
        CreateShapes(puzzle);
        SplitPuzzle();

        gridIconManager.SetTileIcon(puzzle);
    }
    public void ClearPuzzle()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            Destroy(shapes[i].gameObject);
        }
        shapes = new List<Shape>();
        tiles = new List<GameObject>();
    }

    private void CreatePuzzleGrid(PuzzleData _puzzle)
    {
        float posX;
        float posY = ((_puzzle.height / 2) * tileSize) - (tileSize / 2);

        for (int y = 0; y < _puzzle.height; y++)
        {
            posX = -(((_puzzle.width / 2) * tileSize) - (tileSize / 2));
            for (int x = 0; x < _puzzle.width; x++)
            {
                // Create grid
                GameObject tile = Instantiate(tilePrefab, puzzleParent);
                tile.GetComponent<RectTransform>().localPosition = new Vector3(posX, posY, 0);
                tile.GetComponent<RectTransform>().sizeDelta = new Vector2(tileSize, tileSize);
                tiles.Add(tile);

                posX += tileSize;
            }
            posY -= tileSize;
        }
    }
    private void CreateShapes(PuzzleData _puzzle)
    {
        // Join tiles into shapes
        for (int p = 0; p < _puzzle.shapes.Count; p++)
        {
            // Find all tiles with 'p' number value
            List<GameObject> shapeTiles = new List<GameObject>();
            for (int t = 0; t < tiles.Count; t++)
            {
                if (_puzzle.grid[t] == 0)
                {
                    Destroy(tiles[t]);
                }
                else if (_puzzle.grid[t] == p + 1)
                {
                    shapeTiles.Add(tiles[t]);
                }
            }

            // Create shape parent
            GameObject shape = Instantiate(shapeParentPrefab, puzzleParent);

            // Get minimum and maximum x and y values across all the tiles in shape
            float minX = shapeTiles[0].GetComponent<RectTransform>().anchoredPosition.x, maxX = shapeTiles[0].GetComponent<RectTransform>().anchoredPosition.x;
            float minY = shapeTiles[0].GetComponent<RectTransform>().anchoredPosition.y, maxY = shapeTiles[0].GetComponent<RectTransform>().anchoredPosition.y;
            for (int t = 0; t < shapeTiles.Count; t++)
            {
                // Checks if this tile has a smaller x value
                if (shapeTiles[t].GetComponent<RectTransform>().anchoredPosition.x < minX)
                    minX = shapeTiles[t].GetComponent<RectTransform>().anchoredPosition.x;

                // Checks if this tile has a larger x value
                if (shapeTiles[t].GetComponent<RectTransform>().anchoredPosition.x > maxX)
                    maxX = shapeTiles[t].GetComponent<RectTransform>().anchoredPosition.x;

                // Checks if this tile has a smaller y value
                if (shapeTiles[t].GetComponent<RectTransform>().anchoredPosition.y < minY)
                    minY = shapeTiles[t].GetComponent<RectTransform>().anchoredPosition.y;

                // Checks if this tile has a larger y value
                if (shapeTiles[t].GetComponent<RectTransform>().anchoredPosition.y > maxY)
                    maxY = shapeTiles[t].GetComponent<RectTransform>().anchoredPosition.y;
            }

            // Set shape offset position
            shape.GetComponent<RectTransform>().localPosition = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);

            // Make all shape tiles children of shape parent
            for (int t = 0; t < shapeTiles.Count; t++)
            {
                // Set tile is child of shape
                shapeTiles[t].transform.SetParent(shape.transform);
                ChangeTileColour(shapeTiles[t], p + 1);
            }

            Shape s = shape.GetComponent<Shape>();
            s.InitialiseShape(CheckIsLevelComplete, _puzzle.shapes[p]);
            shapes.Add(s);
        }
    }
    private void ChangeTileColour(GameObject _tile, int _value)
    {
        Color tileColor = new Color();
        switch (_value)
        {
            case 1: tileColor = colorPalettes[colorPalette].color1; break;
            case 2: tileColor = colorPalettes[colorPalette].color2; break;
            case 3: tileColor = colorPalettes[colorPalette].color3; break;
            case 4: tileColor = colorPalettes[colorPalette].color4; break;
        }

        _tile.GetComponent<Image>().color = tileColor;
    }
    private void SplitPuzzle()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].SetShapePosition(splitOffset);
        }
    }
    private void CombinePuzzle()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            shapes[i].CompletePiece();
        }

        // Set level is completed
        PuzzlePackSaveData packData = PuzzleLoader.LoadPuzzlePackSaveData(packID);
        packData.isUnlocked = true;
        packData.currentLevel = levelID + 1;
        PuzzleLoader.SavePuzzlePackData(packID, packData);
    }

    public void CheckIsLevelComplete()
    {
        for (int i = 0; i < shapes.Count; i++)
        {
            if (!shapes[i].isCorrectRotation)
            {
                isLevelComplete = false;
                return;
            }
        }

        isLevelComplete = true;
        CombinePuzzle();
        nextButton.SetActive(true);
    }
    public void Button_RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
    public void Button_NextLevel()
    {
        ClearPuzzle();
        CreatePuzzle(packID, levelID + 1);
    }
}
