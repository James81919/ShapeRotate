using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Color Palette", menuName = "Puzzle Customisation/Color Palette")]
public class ColorPalette : ScriptableObject
{
    public string paletteName;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;

    public ColorPalette()
    {
        paletteName = "New Palette";
        color1 = new Color(1, 1, 1, 1);
        color2 = new Color(1, 1, 1, 1);
        color3 = new Color(1, 1, 1, 1);
        color4 = new Color(1, 1, 1, 1);
    }
}
