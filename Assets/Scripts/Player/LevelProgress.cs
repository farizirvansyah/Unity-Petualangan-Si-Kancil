using UnityEngine;

public static class LevelProgress
{
    public static void UnlockLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("Level_" + levelIndex + "_Unlocked", 1);
        PlayerPrefs.Save();
    }

    public static bool IsLevelUnlocked(int levelIndex)
    {
        return PlayerPrefs.GetInt("Level_" + levelIndex + "_Unlocked", levelIndex == 1 ? 1 : 0) == 1;
    }

    public static void SaveStars(int levelIndex, int stars)
    {
        int bestStars = PlayerPrefs.GetInt("Level_" + levelIndex + "_Stars", 0);

        if (stars > bestStars)
        {
            PlayerPrefs.SetInt("Level_" + levelIndex + "_Stars", stars);
            PlayerPrefs.Save();
        }
    }

    public static int GetStars(int levelIndex)
    {
        return PlayerPrefs.GetInt("Level_" + levelIndex + "_Stars", 0);
    }
    public static void ResetProgress(int totalLevel)
    {
        for (int i = 1; i <= totalLevel; i++)
        {
            PlayerPrefs.DeleteKey("Level_" + i + "_Unlocked");
            PlayerPrefs.DeleteKey("Level_" + i + "_Stars");
        }

        PlayerPrefs.Save();
    }
}