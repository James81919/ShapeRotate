using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle Pack", menuName = "Puzzle Customisation/Puzzle Pack")]
public class PuzzlePack : ScriptableObject
{
    public string packName;
    public List<PuzzleData> puzzles;
}

[System.Serializable]
public class PuzzleData
{
    // Level
    public int levelID;
    //public int packID;
    
    // Grid size
    public int width;
    public int height;
    public List<int> grid;
    public List<PuzzleShapeData> shapes;

    // Other
    [HideInInspector]
    public bool isCompleted;

    public PuzzleData(int _levelID, int _width, int _height, List<int> _grid, List<PuzzleShapeData> _shapes)
    {
        levelID = _levelID;
        width = _width;
        height = _height;
        grid = _grid;
        shapes = _shapes;
        isCompleted = false;
    }
}

[System.Serializable]
public class PuzzleShapeData
{
    // Anchor point
    public float anchorPointX;
    public float anchorPointY;

    // Rotation variables
    public RotationDirection startRotation; // 0, 1, 2, 3
    public List<RotationDirection> alternateCorrectRotations;

    public PuzzleShapeData(Vector2 _anchorPoint, RotationDirection _startRotation)
    {
        anchorPointX = _anchorPoint.x;
        anchorPointY = _anchorPoint.y;

        startRotation = _startRotation;

        alternateCorrectRotations = new List<RotationDirection>();
    }

    public PuzzleShapeData(Vector2 _anchorPoint, RotationDirection _startRotation, List<RotationDirection> _alternateCorrectRotations)
    {
        anchorPointX = _anchorPoint.x;
        anchorPointY = _anchorPoint.y;

        startRotation = _startRotation;

        alternateCorrectRotations = _alternateCorrectRotations;
    }
}
