using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RotationDirection
{
    TOP = 0, // 0 degrees
    LEFT = 1, // 90 degrees
    BOTTOM = 2, // 180 degrees
    RIGHT = 3 // 270 degrees
}

public class Shape : MonoBehaviour
{
    public RotationDirection startRotation;
    public Vector2 splitDirection = new Vector2(1, 1);
    public bool isCorrectRotation;

    [SerializeField] private GameObject outlineParent;
    [SerializeField] private GameObject tileOutlinePrefab;

    private RectTransform rectTransform;
    [HideInInspector] public Button button;

    public Action checkIsLevelComplete;

    // Shape size
    private List<bool> grid;
    private int gridWidth;
    private int gridHeight;

    private bool isRotating = false;
    private bool isLocked = false;

    private Vector3 completePosition;
    [HideInInspector] public bool isComplete = false;
    [HideInInspector] public bool isCombined = false;

    private List<bool> gridWithRotation;
    private int rotatedGridWidth;
    private int rotatedGridHeight;

    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(RotateShape);
    }

    public void InitialiseShape(Action _checkIsLevelComplete, PuzzleShapeData _shapeData, List<bool> _tilesGrid, int _shapeWidth, int _shapeHeight)
    {
        checkIsLevelComplete = _checkIsLevelComplete;

        rectTransform = GetComponent<RectTransform>();
        completePosition = rectTransform.localPosition;

        grid = _tilesGrid;
        gridWithRotation = _tilesGrid;

        gridWidth = _shapeWidth;
        gridHeight = _shapeHeight;
        rotatedGridWidth = _shapeWidth;
        rotatedGridHeight = _shapeHeight;

        startRotation = _shapeData.startRotation;

        splitDirection = new Vector2(_shapeData.anchorPointX, _shapeData.anchorPointY);

        // Set rotation
        switch (startRotation)
        {
            case RotationDirection.TOP: rectTransform.rotation = Quaternion.Euler(0, 0, 0); break;
            case RotationDirection.LEFT: rectTransform.rotation = Quaternion.Euler(0, 0, 90); RotateGridWithRotation(3); break;
            case RotationDirection.BOTTOM: rectTransform.rotation = Quaternion.Euler(0, 0, 180); RotateGridWithRotation(2); break;
            case RotationDirection.RIGHT: rectTransform.rotation = Quaternion.Euler(0, 0, 270); RotateGridWithRotation(); break;
        }

        SetupShapeOutline();

        CheckIsCorrectRotation();
    }
    public void RotateShape()
    {
        if (!isRotating && !isComplete && !isLocked)
        {
            isRotating = true;
            AudioManager.instance.PlaySFX("ShapeRotated");
            LeanTween.rotateAround(rectTransform, new Vector3(0, 0, 1), -90, 0.25f).setOnComplete(() =>
            {
                isRotating = false;
                RotateGridWithRotation();
                CheckIsCorrectRotation();
                checkIsLevelComplete();
            });
        }
    }
    public void RotateToCorrectRotation()
    {
        if (!isRotating && !isComplete)
        {
            isRotating = true;
            LeanTween.rotateAround(rectTransform, new Vector3(0, 0, 1), -90, 0.25f).setOnComplete(() =>
            {
                isRotating = false;
                CheckIsCorrectRotation();
                checkIsLevelComplete();
                if (!isCorrectRotation)
                {
                    RotateToCorrectRotation();
                }
                else
                {
                    LockPiece();
                }
            });
        }
    }
    public void SetShapePosition(float _splitOffset)
    {
        transform.localPosition += new Vector3(splitDirection.x, splitDirection.y, 0) * _splitOffset;
    }
    public void CompletePiece()
    {
        isComplete = true;
        DisableOutline();
        LeanTween.move(rectTransform, completePosition, 0.25f).setEase(LeanTweenType.easeInBack).setOnComplete(() => { isCombined = true; });
    }

    public void LockPiece()
    {
        // Lock shape
        isLocked = true;

        // Make outline appear around shape
        EnableOutline();
    }

    private void CheckIsCorrectRotation()
    {
        bool isCorrect = true;
        for (int i = 0; i < grid.Count; i++)
        {
            if (grid[i] != gridWithRotation[i])
            {
                isCorrect = false;
                break;
            }
        }

        isCorrectRotation = isCorrect && gridWidth == rotatedGridWidth && gridHeight == rotatedGridHeight;
    }

    private void RotateGridWithRotation(int _rotations = 1)
    {
        for (int i = 0; i < _rotations; i++)
        {
            List<bool> oldGrid = gridWithRotation;
            gridWithRotation = new List<bool>();
            for (int x = 0; x < rotatedGridWidth; x++)
            {
                for (int y = rotatedGridHeight - 1; y >= 0; y--)
                {
                    int id = x + (rotatedGridWidth * y);
                    gridWithRotation.Add(oldGrid[id]);
                }
            }

            int oldHeight = rotatedGridHeight;
            rotatedGridHeight = rotatedGridWidth;
            rotatedGridWidth = oldHeight;
        }
    }

    private void SetupShapeOutline()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);

            if (child.name != "Outline")
                Instantiate(tileOutlinePrefab, outlineParent.transform).transform.position = child.position;
        }

        DisableOutline();
    }
    private void EnableOutline()
    {
        outlineParent.SetActive(true);
    }
    private void DisableOutline()
    {
        outlineParent.SetActive(false);
    }
}
