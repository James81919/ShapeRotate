using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourPalettesPopup : Popup
{
    [SerializeField] private GameObject colourPaletteItemPrefab;

    [SerializeField] private float listItemOffset;

    [Header("Scroll UI")]
    [SerializeField] private Transform scrollContent;

    [Header("External References")]
    [SerializeField] private LevelManager levelManager;

    private List<ColourPaletteButton> paletteButtons = new List<ColourPaletteButton>();

    public override void OpenPopup()
    {
        base.OpenPopup();

        // Update colour palette selection with bought/not bought
        SetupPaletteList();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < paletteButtons.Count; i++)
        {
            paletteButtons[i].UpdateButtons();
        }
    }

    private void DestroyButtons()
    {
        for (int i = 0; i < paletteButtons.Count; i++)
        {
            Destroy(paletteButtons[i].gameObject);
        }
        paletteButtons.Clear();
    }

    private void SetupPaletteList()
    {
        DestroyButtons();

        int paletteCount = levelManager.colorPalettes.Count;

        for (int i = 0; i < paletteCount; i++)
        {
            SetupPaletteButton(i);
        }

        scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollContent.GetComponent<RectTransform>().sizeDelta.x, (paletteCount * 200) + ((paletteCount - 1) * listItemOffset));

        ColourPaletteManager.SetColourPaletteUnlocked(0, true);
    }
    private void SetupPaletteButton(int _id)
    {
        ColourPaletteButton colourPalette = Instantiate(colourPaletteItemPrefab, scrollContent).GetComponent<ColourPaletteButton>();
        colourPalette.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100 - (_id * (200 + listItemOffset)));
        colourPalette.SetupButton(levelManager, _id);
        colourPalette.onUpdateButtons = UpdateButtons;

        paletteButtons.Add(colourPalette);
    }
}
