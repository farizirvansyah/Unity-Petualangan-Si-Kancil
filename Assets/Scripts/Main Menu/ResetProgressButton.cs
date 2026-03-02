using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetProgressButton : MonoBehaviour
{
    public int totalLevel = 3;
    public string levelMenuSceneName = "Main Menu";

    public void ResetProgress()
    {
        LevelProgress.ResetProgress(totalLevel);
        TutorialController.ResetTutorials();

        // Reload menu supaya UI ke-refresh
        SceneManager.LoadScene(levelMenuSceneName);
    }
}