using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip defaultSFXClick;
    public AudioClip defaultSFXCancel;

    const string SFX = "SFXVolume";

    void Awake()
    {
        // === SINGLE INSTANCE ===
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        // === INIT PREFS ===
        if (!PlayerPrefs.HasKey(SFX))
        {
            PlayerPrefs.SetFloat(SFX, 1f);
            PlayerPrefs.Save();
        }

        ApplyVolume(PlayerPrefs.GetFloat(SFX, 1f));
    }

    // ================== PLAY ==================

    public void PlayClick()
    {
        if (sfxSource != null && defaultSFXClick != null)
            sfxSource.PlayOneShot(defaultSFXClick);
    }

    public void PlayCancel()
    {
        if (sfxSource != null && defaultSFXCancel != null)
            sfxSource.PlayOneShot(defaultSFXCancel);
    }

    // ================== VOLUME ==================

    public void SetSFX(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        ApplyVolume(value);
        PlayerPrefs.SetFloat(SFX, value);
        PlayerPrefs.Save();
    }

    void ApplyVolume(float value)
    {
        audioMixer.SetFloat(SFX, Mathf.Log10(value) * 20f);
    }
}
