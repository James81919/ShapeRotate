using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class PuzzleLoader
{
    public static List<PuzzlePack> LoadPuzzlePacks()
    {
        string path = Application.streamingAssetsPath + "/packs.ldf";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<PuzzlePack> data = formatter.Deserialize(stream) as List<PuzzlePack>;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Puzzle Packs file not found in " + path);
            return null;
        }
    }
    public static PuzzlePack LoadPuzzlePack(int _packID)
    {
        List<PuzzlePack> puzzlePacks = LoadPuzzlePacks();
        if (puzzlePacks != null)
        {
            if (puzzlePacks.Count > _packID)
            {
                return puzzlePacks[_packID];
            }
            else
            {
                Debug.LogError("No puzzle pack found with id of " + _packID);
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    public static PuzzleData LoadPuzzle(int _packID, int _levelID)
    {
        PuzzlePack puzzlePack = LoadPuzzlePack(_packID);
        if (puzzlePack != null)
        {
            if (puzzlePack.puzzles.Count > _levelID)
            {
                return puzzlePack.puzzles[_levelID];
            }
            else
            {
                Debug.LogError("No puzzle found in pack with ID of " + _levelID);
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    public static void SavePuzzlePacks(List<PuzzlePack> _packs)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/packs.ldf";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, _packs);
        stream.Close();
    }
    public static void SavePuzzlePack(int _packID, PuzzlePack _puzzlePack)
    {
        // Load puzzle pack list data
        List<PuzzlePack> packs = LoadPuzzlePacks();

        if (packs != null)
        {
            // Add puzzle pack to list
            if (packs.Count > _packID)
            {
                packs[_packID] = _puzzlePack;
            }
            else
            {
                packs.Add(_puzzlePack);
            }
        }
        else
        {
            // Create new puzzle pack list
            packs = new List<PuzzlePack>();
            packs.Add(_puzzlePack);
        }

        // Save puzzle packs list with added puzzle pack
        SavePuzzlePacks(packs);
    }
    public static void SavePuzzle(int _packID, int _levelID, int _width, int _height, List<int> _grid, List<PuzzleShapeData> _shapes)
    {
        // Create puzzle data
        PuzzleData puzzle = new PuzzleData(_levelID, _width, _height, _grid, _shapes);

        // Load puzzle pack data
        PuzzlePack puzzlePack = LoadPuzzlePack(_packID);
        
        if (puzzlePack != null)
        {
            // Add puzzle to puzzle pack
            if (puzzlePack.puzzles.Count > _levelID)
            {
                puzzlePack.puzzles[_levelID] = puzzle;
            }
            else
            {
                puzzlePack.puzzles.Add(puzzle);
            }
        }
        else
        {
            // Create now puzzle pack
            puzzlePack = new PuzzlePack();
            puzzlePack.packName = "New Pack";
            puzzlePack.puzzles = new List<PuzzleData>();
            puzzlePack.puzzles.Add(puzzle);
        }

        // Save puzzle pack with new puzzle
        SavePuzzlePack(_packID, puzzlePack);
    }

    // Extra functions
    public static List<int> ConvertRotationDirectionsToInts(List<RotationDirection> _rotationDirections)
    {
        List<int> intList = new List<int>();
        for (int i = 0; i < _rotationDirections.Count; i++)
        {
            intList.Add((int)_rotationDirections[i]);
        }
        return intList;
    }
    public static List<RotationDirection> ConvertIntsToRotationDirections(List<int> _intList)
    {
        List<RotationDirection> rotationDirections = new List<RotationDirection>();
        for (int i = 0; i < _intList.Count; i++)
        {
            rotationDirections.Add((RotationDirection)_intList[i]);
        }
        return rotationDirections;
    }
}
