using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ColourPaletteButton : MonoBehaviour
{
    public Action onUpdateButtons;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image colourSample1, colourSample2, colourSample3, colourSample4;
    [SerializeField] private Button buyButton, useButton;

    private LevelManager levelManager;
    private int paletteID;

    public void SetupButton(LevelManager _levelManager, int _paletteID)
    {
        levelManager = _levelManager;
        ColorPalette colorPalette = levelManager.colorPalettes[_paletteID];
        paletteID = _paletteID;

        // Setup name
        nameText.text = colorPalette.paletteName;

        // Setup colour samples
        colourSample1.color = colorPalette.color1;
        colourSample2.color = colorPalette.color2;
        colourSample3.color = colorPalette.color3;
        colourSample4.color = colorPalette.color4;

        UpdateButtons();
    }

    public void UpdateButtons()
    {
        // If the colour palette is unlocked
        if (ColourPaletteManager.IsColourPaletteUnlocked(paletteID))
        {
            buyButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(true);
            useButton.interactable = ColourPaletteManager.GetSelectedColourPalette() != paletteID;
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            useButton.gameObject.SetActive(false);
        }
    }

    public void Button_Buy()
    {
        ColourPaletteManager.SetColourPaletteUnlocked(paletteID, true);
        ColourPaletteManager.SetSelectedColourPalette(paletteID);
        onUpdateButtons();
    }

    public void Button_Use()
    {
        ColourPaletteManager.SetSelectedColourPalette(paletteID);
        onUpdateButtons();
    }
}
