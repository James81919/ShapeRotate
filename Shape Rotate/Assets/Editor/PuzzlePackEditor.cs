using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PuzzlePack))]
public class PuzzlePackEditor : Editor
{
    int gridWidth, gridHeight;
    List<int> gridList = new List<int>();


    List<PuzzleShapeData> shapesList = new List<PuzzleShapeData>();
    int shapeListCount = 0;
    List<bool> isShapeElementTabsOpen = new List<bool>();

    int newPuzzleLevelID = 0;
    int editingLevelID = 0;
    bool isEditingLevel = false;

    PuzzlePack puzzlePack;

    bool isShapeListTabOpen = false;

    public override void OnInspectorGUI()
    {
        puzzlePack = (PuzzlePack)target;

        EditorGUILayout.BeginHorizontal();
        editingLevelID = EditorGUILayout.IntField(editingLevelID);
        if (GUILayout.Button("Edit Level"))
        {
            if (puzzlePack.puzzles.Count > editingLevelID)
            {
                isEditingLevel = true;
                LoadPuzzle();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        isShapeListTabOpen = EditorGUILayout.Foldout(isShapeListTabOpen, "Shapes");
        shapeListCount = EditorGUILayout.IntField(shapeListCount);
        EditorGUILayout.EndHorizontal();

        // Update size of shapes list
        while (shapesList.Count < shapeListCount)
        {
            shapesList.Add(new PuzzleShapeData(new Vector2(0, 0), RotationDirection.TOP));
        }
        while (shapesList.Count > shapeListCount)
        {
            shapesList.RemoveAt(shapesList.Count - 1);
        }

        // Update size of of is shape element tabs open list
        while (isShapeElementTabsOpen.Count < shapeListCount)
        {
            isShapeElementTabsOpen.Add(false);
        }
        while (isShapeElementTabsOpen.Count > shapeListCount)
        {
            isShapeElementTabsOpen.RemoveAt(isShapeElementTabsOpen.Count - 1);
        }

        EditorGUILayout.BeginVertical(GUI.skin.GetStyle("helpBox"));
        if (isShapeListTabOpen)
        {
            for (int i = 0; i < shapesList.Count; i++)
            {
                isShapeElementTabsOpen[i] = EditorGUILayout.Foldout(isShapeElementTabsOpen[i], "Shape" + (i + 1));

                if (isShapeElementTabsOpen[i])
                {
                    shapesList[i].anchorPointX = EditorGUILayout.FloatField("Anchor Point X", shapesList[i].anchorPointX);
                    shapesList[i].anchorPointY = EditorGUILayout.FloatField("Anchor Point Y", shapesList[i].anchorPointY);
                    shapesList[i].startRotation = (RotationDirection)EditorGUILayout.EnumPopup("Start Rotation", shapesList[i].startRotation);

                    EditorGUILayout.Space();
                }
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (shapesList.Count > 0)
        {
            // Set width and height of grid
            gridWidth = EditorGUILayout.IntField("Width", gridWidth);
            gridHeight = EditorGUILayout.IntField("Height", gridHeight);

            int buttonSize = gridWidth > 0 ? ((Screen.width - 100) / gridWidth) : 0;

            // Render grid of buttons
            for (int h = 0; h < gridHeight; h++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int w = 0; w < gridWidth; w++)
                {
                    int index = (h * gridWidth) + w;

                    while (gridList.Count <= index)
                    {
                        gridList.Add(0);
                    }

                    SetTileColor(index);
                    if (GUILayout.Button(gridList[index].ToString(), GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                    {
                        ChangeTileValue(index);
                    }
                }
                GUI.backgroundColor = Color.white;

                EditorGUILayout.EndHorizontal();
            }

            while (gridList.Count > gridWidth * gridHeight)
            {
                gridList.RemoveAt(gridList.Count - 1);
            }

            if (gridWidth > 0 && gridHeight > 0)
            {
                if (DoesPuzzleExist())
                {
                    // Make text colour red
                    EditorGUILayout.LabelField("WARNING: Puzzle Exists!");
                }
                else
                {
                    // Make text colour green
                    EditorGUILayout.LabelField("Puzzle doesn't exist");

                    EditorGUILayout.Space();
                    if (isEditingLevel)
                    {
                        if (GUILayout.Button("Save"))
                        {
                            AddPuzzle(editingLevelID, true);
                            isEditingLevel = false;
                        }
                        if (GUILayout.Button("Deselect"))
                        {
                            isEditingLevel = false;
                        }
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        newPuzzleLevelID = EditorGUILayout.IntSlider(newPuzzleLevelID, 1, puzzlePack.puzzles.Count);
                        if (GUILayout.Button("Add Puzzle By Level Number"))
                        {
                            AddPuzzle(newPuzzleLevelID - 1);
                            newPuzzleLevelID++;
                        }
                        EditorGUILayout.EndHorizontal();


                        EditorGUILayout.Space();
                        if (GUILayout.Button("Add Puzzle"))
                        {
                            AddPuzzle();
                        }
                    }
                }
            }
        }

        EditorGUILayout.Space();
        
        base.OnInspectorGUI();
    }

    private void ChangeTileValue(int _gridIndex)
    {
        if (shapesList.Count == 0)
        {
            gridList[_gridIndex] = 0;
            return;
        }

        if (gridList[_gridIndex] < shapesList.Count)
        {
            gridList[_gridIndex]++;
        }
        else
        {
            gridList[_gridIndex] = 0;
        }
    }

    private void SetTileColor(int _gridIndex)
    {
        switch (gridList[_gridIndex])
        {
            case 1: GUI.backgroundColor = new Color(0, 0.4745098f, 0.8039216f); break; // Set colour blue
            case 2: GUI.backgroundColor = new Color(0.8039216f, 0.509804f, 0); break; // Set colour yellow
            case 3: GUI.backgroundColor = new Color(0.2509804f, 0.7568628f, 0); break; // Set colour green
            case 4: GUI.backgroundColor = new Color(0.8117648f, 0, 0); break; // Set colour red
            default: GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f); break; // Set colour grey (by default)
        }
    }

    private void AddPuzzle(int _levelID = -1, bool _override = false)
    {
        if (puzzlePack == null)
            return;

        // Create and add puzzle to puzzlepack
        List<PuzzleShapeData> newShapesList = new List<PuzzleShapeData>();
        for (int i = 0; i < shapesList.Count; i++)
        {
            PuzzleShapeData s = new PuzzleShapeData(
                new Vector2(shapesList[i].anchorPointX, shapesList[i].anchorPointY),
                shapesList[i].startRotation);

            newShapesList.Add(s);
        }

        PuzzleData puzzle = new PuzzleData(gridWidth, gridHeight, gridList, newShapesList);
        if (_override)
        {
            puzzlePack.puzzles[_levelID] = puzzle;
        }
        else
        {
            if (_levelID >= 0)
                puzzlePack.puzzles.Insert(_levelID, puzzle);
            else
                puzzlePack.puzzles.Add(puzzle);

        }

        // Reset local variables
        gridList = new List<int>();
    }

    private void LoadPuzzle()
    {
        if (puzzlePack == null)
            return;

        if (puzzlePack.puzzles.Count <= editingLevelID)
            return;

        // Get existing puzzle from puzzlepack
        PuzzleData puzzle = puzzlePack.puzzles[editingLevelID];

        gridWidth = puzzle.width;
        gridHeight = puzzle.height;

        gridList = new List<int>();
        for (int g = 0; g < puzzle.grid.Count; g++)
        {
            gridList.Add(puzzle.grid[g]);
        }

        shapesList = new List<PuzzleShapeData>();
        for (int i = 0; i < puzzle.shapes.Count; i++)
        {
            PuzzleShapeData newShape = new PuzzleShapeData(new Vector2(puzzle.shapes[i].anchorPointX, puzzle.shapes[i].anchorPointY), puzzle.shapes[i].startRotation);
            shapesList.Add(newShape);
        }

        shapeListCount = puzzle.shapes.Count;
    }

    private bool DoesPuzzleExist()
    {
        PuzzlePack pack = (PuzzlePack)target;

        for (int i = 0; i < pack.puzzles.Count; i++)
        {
            if (DoListsMatch(pack.puzzles[i].grid, gridList))
            {
                // Puzzle Exists
                return !isEditingLevel || (isEditingLevel && editingLevelID != i);
            }
        }

        return false;
    }
    private bool DoListsMatch(List<int> _list1, List<int> _list2)
    {
        if (_list1.Count != _list2.Count)
            return false;

        for (int i = 0; i < _list1.Count; i++)
        {
            if (_list1[i] != _list2[i])
                return false;
        }

        return true;
    }
}
