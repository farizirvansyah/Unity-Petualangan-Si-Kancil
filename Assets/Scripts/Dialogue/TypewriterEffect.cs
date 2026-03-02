using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.04f;

    TextMeshProUGUI textComponent;
    string fullText;
    Coroutine typingCoroutine;
    bool isTyping;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void StartTyping(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        fullText = text;
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        textComponent.text = "";

        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    public void SkipTyping()
    {
        if (!isTyping) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        textComponent.text = fullText;
        isTyping = false;
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}
