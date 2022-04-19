using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColourPaletteManager
{
    public static void SetColourPaletteUnlocked(int _id, bool _isUnlocked)
    {
        PlayerPrefs.SetInt("IsColourPaletteUnlocked_" + _id, _isUnlocked ? 1 : 0);
    }

    public static bool IsColourPaletteUnlocked(int _id)
    {
        return PlayerPrefs.GetInt("IsColourPaletteUnlocked_" + _id, 0) == 1;
    }

    public static void SetSelectedColourPalette(int _id)
    {
        PlayerPrefs.SetInt("SelectedColourPalette", _id);
    }

    public static int GetSelectedColourPalette()
    {
        return PlayerPrefs.GetInt("SelectedColourPalette", 0);
    }
}
