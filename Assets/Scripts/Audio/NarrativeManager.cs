using UnityEngine;
using UnityEngine.Audio;

public class NarrativeManager : MonoBehaviour
{
    public static NarrativeManager instance;

    [Header("Mixer")]
    public AudioMixer audioMixer;
    public AudioSource narrativeSource;
    const string NARRATIVE = "NarrativeVolume";

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey(NARRATIVE))
        {
            PlayerPrefs.SetFloat(NARRATIVE, 1f);
            PlayerPrefs.Save();
        }

        ApplyVolume(PlayerPrefs.GetFloat(NARRATIVE));
    }

    public void SetNarrative(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        audioMixer.SetFloat(NARRATIVE, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(NARRATIVE, value);
        PlayerPrefs.Save();
    }

    void ApplyVolume(float value)
    {
        audioMixer.SetFloat(NARRATIVE, Mathf.Log10(value) * 20);
    }
}
