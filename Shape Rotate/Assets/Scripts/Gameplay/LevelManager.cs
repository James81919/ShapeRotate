using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ColorPalette
{
    public string paletteName;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
}

public class LevelManager : MonoBehaviour
{
    public List<ColorPalette> colorPalettes;
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
        Invoke("CreateFirstPuzzle", Time.deltaTime);
    }

    // TEMP Function!!
    private void CreateFirstPuzzle()
    {
        CreatePuzzle(1, 1);
    }

    public void CreatePuzzle(int _packID, int _levelID)
    {
        nextButton.SetActive(false);

        packID = _packID;
        levelID = _levelID;

        //PuzzleData puzzle = PuzzleLoader.LoadPuzzle(_packID, _levelID);
        //if (puzzle == null)
        //{
            List<int> grid = new List<int>();
            grid.Add(1);
            grid.Add(1);
            grid.Add(2);
            grid.Add(2);

            grid.Add(1);
            grid.Add(3);
            grid.Add(2);
            grid.Add(2);

            grid.Add(3);
            grid.Add(3);
            grid.Add(2);
            grid.Add(4);

            grid.Add(3);
            grid.Add(4);
            grid.Add(4);
            grid.Add(4);

            List<PuzzleShapeData> shapeData = new List<PuzzleShapeData>();
            shapeData.Add(new PuzzleShapeData(new Vector2(-1, 1), RotationDirection.LEFT));
            shapeData.Add(new PuzzleShapeData(new Vector2(1, 1), RotationDirection.RIGHT));
            List<RotationDirection> shape3AltCorrectRotations = new List<RotationDirection>();
            shape3AltCorrectRotations.Add(RotationDirection.BOTTOM);
            shapeData.Add(new PuzzleShapeData(new Vector2(-1, -1), RotationDirection.BOTTOM, shape3AltCorrectRotations));
            shapeData.Add(new PuzzleShapeData(new Vector2(1, -1), RotationDirection.LEFT));

            //puzzle = new PuzzleData(0, 4, 4, grid, shapeData);
        //}

        PuzzleLoader.SavePuzzle(0, 0, 4, 4, grid, shapeData);
        PuzzleData puzzle = PuzzleLoader.LoadPuzzle(0, 0);

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
            Destroy(shapes[i]);
        }
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
        LevelLoader.Instance.LoadLevel("game");
    }
    public void Button_NextLevel()
    {
        LevelLoader.Instance.Fade(() => {
            ClearPuzzle();
            CreatePuzzle(packID, levelID + 1);
        });
    }
}
