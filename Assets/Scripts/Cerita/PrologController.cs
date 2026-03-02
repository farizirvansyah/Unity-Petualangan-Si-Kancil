using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PrologController : MonoBehaviour
{
    public NarrativeController narrativeController;
    public GameObject prologPanel;
    public TextMeshProUGUI textJudul;
    public TypewriterEffect typewriter;
    public AudioSource audioSource;

    Coroutine autoSkipCoroutine;

    void Start()
    {
        if (NarrativeSettings.IsSkipEnabled())
        {
            prologPanel.SetActive(false);
            Time.timeScale = 1f;
            return;
        }

        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        LevelData data = LevelDatabase.instance.GetLevelData(levelIndex);

        narrativeController.Play(
        data.prologSlides,
        () => Time.timeScale = 1f
    );

        // if (data == null) return;

        // textJudul.text = data.judulLevel;
        // prologPanel.SetActive(true);
        // Time.timeScale = 0f;

        // typewriter.StartTyping(data.prologText);

        // if (data.prologVoice != null)
        // {
        //     audioSource.clip = data.prologVoice;
        //     audioSource.Play();
        //     autoSkipCoroutine = StartCoroutine(AutoContinueAfterAudio());
        // }
        // else
        // {
        //     autoSkipCoroutine = StartCoroutine(AutoContinueAfterText());
        // }
    }

    IEnumerator AutoContinueAfterAudio()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        MulaiLevel();
    }

    IEnumerator AutoContinueAfterText()
    {
        yield return new WaitUntil(() => !typewriter.IsTyping());
        yield return new WaitForSecondsRealtime(1f);
        MulaiLevel();
    }

    public void MulaiLevel()
    {
        if (autoSkipCoroutine != null)
            StopCoroutine(autoSkipCoroutine);

        typewriter.SkipTyping();
        audioSource.Stop();
        prologPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
