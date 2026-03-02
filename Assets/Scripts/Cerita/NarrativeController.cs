using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NarrativeController : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public Image slideImage;
    public TypewriterEffect typewriter;
    public AudioSource audioSource;

    [Header("Overlay")]
    public CanvasGroup blackOverlay;
    public float fadeDuration = 0.5f;

    List<NarrativeSlide> slides;
    int slideIndex = 0;
    int lineIndex = 0;

    bool isSliding = false;
    Coroutine autoCoroutine;
    System.Action onFinishCallback;

    // ===================== PUBLIC =====================

    public void Play(List<NarrativeSlide> slideData, System.Action onFinish)
    {
        slides = slideData;
        slideIndex = 0;
        lineIndex = 0;
        onFinishCallback = onFinish;

        panel.SetActive(true);
        Time.timeScale = 0f;

        // Mulai dari layar hitam
        blackOverlay.gameObject.SetActive(true);
        blackOverlay.alpha = 1f;

        ShowSlide();

        // Fade dari hitam → tampil slide
        StartCoroutine(FadeOverlay(1f, 0f));
    }

    public void Next()
    {
        if (isSliding) return;

        if (typewriter.IsTyping())
        {
            typewriter.SkipTyping();
            return;
        }

        StopAudio();

        if (lineIndex < slides[slideIndex].lines.Count - 1)
        {
            lineIndex++;
            ShowLine();
        }
        else
        {
            StartCoroutine(NextSlide());
        }
    }

    // ===================== LINE =====================

    void ShowLine()
    {
        NarrativeLine line = slides[slideIndex].lines[lineIndex];

        typewriter.StartTyping(line.text);

        if (line.voice != null)
        {
            audioSource.clip = line.voice;
            audioSource.Play();
            autoCoroutine = StartCoroutine(AutoNextAfterAudio());
        }
    }

    IEnumerator AutoNextAfterAudio()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        Next();
    }

    // ===================== SLIDE =====================

    void ShowSlide()
    {
        slideImage.sprite = slides[slideIndex].image;
        lineIndex = 0;
        ShowLine();
    }

    IEnumerator NextSlide()
    {
        isSliding = true;

        // Fade ke hitam
        yield return StartCoroutine(FadeOverlay(0f, 1f));

        slideIndex++;

        if (slideIndex >= slides.Count)
        {
            Finish();
            yield break;
        }

        ShowSlide();

        // Fade dari hitam
        yield return StartCoroutine(FadeOverlay(1f, 0f));

        isSliding = false;
    }

    // ===================== UTILS =====================

    IEnumerator FadeOverlay(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            blackOverlay.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        blackOverlay.alpha = to;
    }

    void StopAudio()
    {
        if (autoCoroutine != null)
            StopCoroutine(autoCoroutine);

        audioSource.Stop();
    }

    void OnDisable()
    {
        if (blackOverlay != null)
            blackOverlay.gameObject.SetActive(false);
    }

    void Finish()
    {
        StopAudio();

        blackOverlay.alpha = 0f;
        blackOverlay.gameObject.SetActive(false);

        panel.SetActive(false);
        Time.timeScale = 1f;
        onFinishCallback?.Invoke();
    }
}