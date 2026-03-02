using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject panelPause;
    public Button buttonPause;
    public Button pauseHome;
    public Button pauseRetry;
    public Button pauseResume;
    void Start()
    {
        buttonPause.onClick.AddListener(PauseGame);
        pauseHome.onClick.AddListener(Home);
        pauseRetry.onClick.AddListener(Retry);
        pauseResume.onClick.AddListener(ResumeGame);
    }
    void PauseGame()
    {
        Time.timeScale = 0f;
        buttonPause.gameObject.SetActive(false);
        panelPause.SetActive(true);
    }
    void ResumeGame()
    {
        Time.timeScale = 1f;
        buttonPause.gameObject.SetActive(true);
        panelPause.SetActive(false);
    }
    void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
    void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}