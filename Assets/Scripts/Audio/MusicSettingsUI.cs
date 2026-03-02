using UnityEngine;
using UnityEngine.UI;

public class MusicSettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    const string MUSIC = "MusicVolume";

    void Start()
    {
        musicSlider.minValue = 0.0001f;
        musicSlider.maxValue = 1f;

        float value = PlayerPrefs.GetFloat(MUSIC, 1f);
        musicSlider.value = value;

        musicSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.AddListener(MusicManager.instance.SetMusic);
    }
}
