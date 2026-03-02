using UnityEngine;

[System.Serializable]
public class NarrativeLine
{
    [TextArea(3, 10)]
    public string text;

    public AudioClip voice;
}
