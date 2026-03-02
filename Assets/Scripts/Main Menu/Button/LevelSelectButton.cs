using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    public int levelIndex;
    public Button button;
    public Image[] stars;
    public GameObject lockIcon;

    void Start()
    {
        bool unlocked = LevelProgress.IsLevelUnlocked(levelIndex);

        button.interactable = unlocked;
        lockIcon.SetActive(!unlocked);

        int starCount = LevelProgress.GetStars(levelIndex);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = i < starCount;
        }
    }

    public void LoadLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }
}
