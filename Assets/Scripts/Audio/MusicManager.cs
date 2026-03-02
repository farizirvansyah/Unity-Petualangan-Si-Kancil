using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Music")]
    public AudioSource musicSource;
    public AudioClip defaultBGM;

    [Header("Crossfade")]
    public float fadeDuration = 1f;

    [Header("Scene BGM")]
    public List<SceneBGM> sceneBGM = new List<SceneBGM>();

    const string MUSIC = "MusicVolume";

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
        if (!PlayerPrefs.HasKey(MUSIC))
        {
            PlayerPrefs.SetFloat(MUSIC, 1f);
            PlayerPrefs.Save();
        }

        ApplyVolume(PlayerPrefs.GetFloat(MUSIC, 1f));

        // === SCENE EVENT ===
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        // === PLAY FIRST BGM ===
        PlayBGMForScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ================== CORE ==================

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMForScene(scene.name);
    }

    void PlayBGMForScene(string sceneName)
    {
        AudioClip targetClip = defaultBGM;

        foreach (var item in sceneBGM)
        {
            if (item.sceneName == sceneName)
            {
                targetClip = item.bgm;
                break;
            }
        }

        if (musicSource.clip == targetClip && musicSource.isPlaying)
            return;

        StopAllCoroutines();
        StartCoroutine(CrossfadeBGM(targetClip));
    }

    // ================== CROSSFADE ==================

    IEnumerator CrossfadeBGM(AudioClip newClip)
    {
        float startVolume = musicSource.volume;

        // === FADE OUT ===
        float t = 0f;
        while (t < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        musicSource.volume = 0f;

        // === GANTI CLIP ===
        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();

        // === FADE IN ===
        t = 0f;
        while (t < fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        musicSource.volume = startVolume;
    }

    // ================== VOLUME ==================

    public void SetMusic(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        ApplyVolume(value);
        PlayerPrefs.SetFloat(MUSIC, value);
        PlayerPrefs.Save();
    }

    void ApplyVolume(float value)
    {
        audioMixer.SetFloat(MUSIC, Mathf.Log10(value) * 20f);
    }
}
[System.Serializable]
public class SceneBGM
{
    public string sceneName;
    public AudioClip bgm;
}