using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SimpleLevelButton : MonoBehaviour
{
    public int levelIndex;
    public Button button;
    public Image[] stars;
    public TextMeshProUGUI levelText;

    void Start()
    {
        bool unlocked = LevelProgress.IsLevelUnlocked(levelIndex);

        // KUNCI / BUKA LEVEL
        button.interactable = unlocked;

        // TAMPILKAN NOMOR LEVEL
        levelText.text = levelIndex.ToString();

        // TAMPILKAN BINTANG
        int starCount = LevelProgress.GetStars(levelIndex);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = i < starCount;
        }

        button.onClick.AddListener(LoadLevel);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
