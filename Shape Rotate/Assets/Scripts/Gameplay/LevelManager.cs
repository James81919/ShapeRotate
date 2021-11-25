using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [HideInInspector] public bool isLevelComplete;

    private List<GameObject> tiles = new List<GameObject>();
    private List<Shape> shapes = new List<Shape>();
    private float tileSize = 75;

    private float maxPuzzleSize = 300;

    private int packID, levelID;

    private void Start()
    {
        // TEMP
        Invoke("CreateFirstPuzzle", Time.deltaTime);
    }

    // TEMP Function!!
    private void CreateFirstPuzzle()
    {
        CreatePuzzle(0, 0);
    }

    public void CreatePuzzle(int _packID, int _levelID)
    {
        if (puzzlePacks.Count <= _packID)
            return;

        if (puzzlePacks[_packID].puzzles.Count <= _levelID)
            return;

        nextButton.SetActive(false);

        packID = _packID;
        levelID = _levelID;

        PuzzleData puzzle = puzzlePacks[_packID].puzzles[_levelID];

        // Set tile size
        tileSize = (puzzle.width > puzzle.height) ? maxPuzzleSize / puzzle.width : maxPuzzleSize / puzzle.height;

        CreatePuzzleGrid(puzzle);
        CreateShapes(puzzle);
        SplitPuzzle();
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
            GameObject shape = Instantiate(shapeParentPrefab, puzzleParent);

            // Set shape offset position
            shape.GetComponent<RectTransform>().localPosition = new Vector3(tileSize * _puzzle.shapes[p].anchorPointX, tileSize * _puzzle.shapes[p].anchorPointY, 0);

            // Find all tiles 'p' number value and make them children of shape
            for (int t = 0; t < tiles.Count; t++)
            {
                if (_puzzle.grid[t] == p + 1)
                {
                    // Set tile is child of shape
                    tiles[t].transform.SetParent(shape.transform);
                    ChangeTileColour(tiles[t], p + 1);
                }
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
