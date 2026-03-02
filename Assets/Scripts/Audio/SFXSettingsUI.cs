using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SFXSettingsUI : MonoBehaviour
{
    public Slider sfxSlider;

    [Header("Buttons - Click")]
    public List<Button> clickButtons = new List<Button>();

    [Header("Buttons - Cancel")]
    public List<Button> cancelButtons = new List<Button>();

    const string SFX = "SFXVolume";

    void Start()
    {
        // === SLIDER SETUP ===
        sfxSlider.minValue = 0.0001f;
        sfxSlider.maxValue = 1f;

        float value = PlayerPrefs.GetFloat(SFX, 1f);
        sfxSlider.value = value;

        sfxSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.AddListener(SFXManager.instance.SetSFX);

        // === BUTTON REGISTER ===
        RegisterButtons();
    }

    void RegisterButtons()
    {
        foreach (Button btn in clickButtons)
        {
            if (btn != null)
                btn.onClick.AddListener(SFXManager.instance.PlayClick);
        }

        foreach (Button btn in cancelButtons)
        {
            if (btn != null)
                btn.onClick.AddListener(SFXManager.instance.PlayCancel);
        }
    }
}
