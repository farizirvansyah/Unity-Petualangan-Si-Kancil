using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EpilogController : MonoBehaviour
{
    public NarrativeController narrativeController;
    public GameObject epilogPanel;
    public TypewriterEffect typewriter;
    public AudioSource audioSource;
    public ResultUI resultUI;

    int bintangDisimpan;
    Coroutine autoSkipCoroutine;

    public void ShowEpilog(int bintang)
    {
        if (NarrativeSettings.IsSkipEnabled())
        {
            resultUI.TampilkanResult(bintang);
            return;
        }

        bintangDisimpan = bintang;

        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        LevelData data = LevelDatabase.instance.GetLevelData(levelIndex);

        narrativeController.Play(
        data.epilogSlides,
        () => resultUI.TampilkanResult(bintangDisimpan)
    );

        // if (data == null) return;

        // epilogPanel.SetActive(true);
        // Time.timeScale = 0f;

        // typewriter.StartTyping(data.epilogText);

        // if (data.epilogVoice != null)
        // {
        //     audioSource.clip = data.epilogVoice;
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
        LanjutKeResult();
    }

    IEnumerator AutoContinueAfterText()
    {
        yield return new WaitUntil(() => !typewriter.IsTyping());
        yield return new WaitForSecondsRealtime(1f);
        LanjutKeResult();
    }

    public void LanjutKeResult()
    {
        if (autoSkipCoroutine != null)
            StopCoroutine(autoSkipCoroutine);

        typewriter.SkipTyping();
        audioSource.Stop();
        epilogPanel.SetActive(false);
        Time.timeScale = 1f;

        resultUI.TampilkanResult(bintangDisimpan);
    }
}
