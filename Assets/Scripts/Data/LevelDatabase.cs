using UnityEngine;

public class LevelDatabase : MonoBehaviour
{
    public static LevelDatabase instance;
    public LevelData[] allLevels;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public LevelData GetLevelData(int levelIndex)
    {
        foreach (var data in allLevels)
        {
            if (data.levelIndex == levelIndex)
                return data;
        }
        return null;
    }
}
