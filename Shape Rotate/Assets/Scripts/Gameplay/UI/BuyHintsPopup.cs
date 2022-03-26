using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyHintsPopup : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Canvas popupCanvas;
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        popupCanvas.enabled = false;
    }

    public void Appear()
    {
        popupCanvas.enabled = true;
    }

    public void Disappear()
    {
        popupCanvas.enabled = false;
    }


    // UI Buttons
    public void Button_BuyHints_5()
    {
        HintManager.AddHints(5);
        levelManager.UpdateHintCounter();
    }

    public void Button_BuyHints_20()
    {
        HintManager.AddHints(20);
        levelManager.UpdateHintCounter();
    }

    public void Button_BuyHints_100()
    {
        HintManager.AddHints(100);
        levelManager.UpdateHintCounter();
    }
}
