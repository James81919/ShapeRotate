using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public struct PuzzlePackSaveData
{
    public bool isUnlocked;
    public int currentLevel;
}

public static class PuzzleLoader
{
    public static List<PuzzlePackSaveData> LoadPuzzlePacks()
    {
        string path = Application.persistentDataPath + "/packs.ldf";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            List<PuzzlePackSaveData> data = formatter.Deserialize(stream) as List<PuzzlePackSaveData>;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Puzzle Packs file not found in " + path);
            return null;
        }
    }

    public static PuzzlePackSaveData LoadPuzzlePackSaveData(int _packID)
    {
        List<PuzzlePackSaveData> puzzlePacks = LoadPuzzlePacks();
        if (puzzlePacks != null)
        {
            if (puzzlePacks.Count > _packID)
            {
                return puzzlePacks[_packID];
            }
            else
            {
                Debug.LogError("No puzzle pack found with id of " + _packID);
                return new PuzzlePackSaveData();
            }
        }
        else
        {
            return new PuzzlePackSaveData();
        }
    }

    public static void SavePuzzlePacks(List<PuzzlePackSaveData> _packs)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/packs.ldf";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, _packs);
        stream.Close();
    }

    public static void SavePuzzlePackData(int _packID, PuzzlePackSaveData _puzzlePack)
    {
        // Load puzzle pack list data
        List<PuzzlePackSaveData> packs = LoadPuzzlePacks();

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
            packs = new List<PuzzlePackSaveData>();
            packs.Add(_puzzlePack);
        }

        // Save puzzle packs list with added puzzle pack
        SavePuzzlePacks(packs);
    }

    public static bool IsLevelUnlocked(int _packID, int _levelID)
    {
        PuzzlePackSaveData packData = LoadPuzzlePackSaveData(_packID);
        return _levelID >= packData.currentLevel && packData.isUnlocked;
    }

    public static void UpdateCompletedLevels(int _packID, int _completedLevel)
    {
        PuzzlePackSaveData packData = LoadPuzzlePackSaveData(_packID);

        if (packData.isUnlocked)
        {
            if (_completedLevel == packData.currentLevel)
            {
                packData.currentLevel++;
                SavePuzzlePackData(_packID, packData);
            }

        }
    }

    public static void SetPackUnlocked(int _packID, bool _isUnlocked)
    {
        PuzzlePackSaveData packData = LoadPuzzlePackSaveData(_packID);
        packData.isUnlocked = _isUnlocked;

        SavePuzzlePackData(_packID, packData);
    }
}
