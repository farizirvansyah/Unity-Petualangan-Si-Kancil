using UnityEngine;
using UnityEngine.UI;

public class NarrativeSettingsUI : MonoBehaviour
{
    public Slider narrativeSlider;
    const string NARRATIVE = "NarrativeVolume";

    void Start()
    {
        narrativeSlider.minValue = 0.0001f;
        narrativeSlider.maxValue = 1f;

        float value = PlayerPrefs.GetFloat(NARRATIVE, 1f);
        narrativeSlider.value = value;

        narrativeSlider.onValueChanged.RemoveAllListeners();
        narrativeSlider.onValueChanged.AddListener(NarrativeManager.instance.SetNarrative);
    }
}
