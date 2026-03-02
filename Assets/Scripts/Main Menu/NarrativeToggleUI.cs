using UnityEngine;
using UnityEngine.UI;

public class NarrativeToggleUI : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
        toggle.isOn = NarrativeSettings.IsSkipEnabled();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool value)
    {
        NarrativeSettings.SetSkip(value);
    }
}
