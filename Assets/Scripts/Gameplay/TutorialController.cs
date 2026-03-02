using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [Header("Tutorial Setting")]
    public int targetLevelIndex = 1; // Level 1 (sesuaikan build index)
    public GameObject tutorialPanel;

    const string TUTORIAL_KEY = "TutorialLevel1Shown";

    void Start()
    {
        tutorialPanel.SetActive(false);

        // Hanya untuk level target
        if (SceneManager.GetActiveScene().buildIndex != targetLevelIndex)
            return;

        // Jika sudah pernah ditampilkan, jangan tampilkan lagi
        if (PlayerPrefs.GetInt(TUTORIAL_KEY, 0) == 1)
            return;

        ShowTutorial();
    }

    // ===================== CORE =====================

    void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnClickContinue()
    {
        PlayerPrefs.SetInt(TUTORIAL_KEY, 1);
        PlayerPrefs.Save();

        tutorialPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public static void ResetTutorials()
    {
        PlayerPrefs.DeleteKey(TUTORIAL_KEY);
    }
}
