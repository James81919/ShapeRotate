using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Popup
{
    public Slider musicSlider;
    public Slider soundSlider;

    public ResetGamePopup resetGamePopup;

    public override void OpenPopup()
    {
        UpdateUI();

        base.OpenPopup();
    }

    private void UpdateUI()
    {
        // Set up music slider
        musicSlider.value = AudioManager.instance.GetMusicVolume();
        musicSlider.maxValue = 1;
        musicSlider.onValueChanged.AddListener(delegate { UpdateMusicSliderValue(); });

        // Set up sfx slider
        soundSlider.value = AudioManager.instance.GetSoundVolume();
        soundSlider.maxValue = 1;
        soundSlider.onValueChanged.AddListener(delegate { UpdateSoundSliderValue(); });
    }

    public void UpdateSoundSliderValue()
    {
        AudioManager.instance.SetSFXVolume(soundSlider.value);
    }

    public void UpdateMusicSliderValue()
    {
        AudioManager.instance.SetMusicVolume(musicSlider.value);
    }


    // UI Buttons
    public void Button_RestorePurchases()
    {
        // Restores previous IAP purchases the player has made in the past
    }
    public void Button_Reset()
    {
        resetGamePopup.OpenPopup();
    }
}
