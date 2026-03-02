using UnityEngine;

public static class NarrativeSettings
{
    const string SKIP_NARRATIVE_KEY = "SkipNarrative";

    public static bool IsSkipEnabled()
    {
        return PlayerPrefs.GetInt(SKIP_NARRATIVE_KEY, 0) == 1;
    }

    public static void SetSkip(bool value)
    {
        PlayerPrefs.SetInt(SKIP_NARRATIVE_KEY, value ? 1 : 0);
        PlayerPrefs.Save();
    }
}
