using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip winClip;
    public AudioClip loseClip;
    [Header("UI Elements")]
    [SerializeField] GameObject panelResult;
    [Header("Buttons")]
    public Button buttonHome;
    public Button buttonRetry;
    public Button buttonNext;

    public TextMeshProUGUI textResult;
    public Image star1;
    public Image star2;
    public Image star3;

    void Start()
    {
        panelResult.SetActive(false);

        buttonHome.onClick.AddListener(Home);
        buttonRetry.onClick.AddListener(Retry);
        buttonNext.onClick.AddListener(NextLevel);

        buttonNext.gameObject.SetActive(false);
    }

    public void TampilkanResult(int bintang)
    {
        panelResult.SetActive(true);
        Time.timeScale = 0f;

        buttonNext.gameObject.SetActive(bintang > 0);

        star1.enabled = bintang >= 1;
        star2.enabled = bintang >= 2;
        star3.enabled = bintang >= 3;

        textResult.text = bintang > 0 ? "menang" : "kalah";
        audioSource.PlayOneShot(bintang > 0 ? winClip : loseClip);

        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (bintang > 0)
        {
            // SIMPAN BINTANG TERBAIK
            LevelProgress.SaveStars(currentLevel, bintang);

            // BUKA LEVEL BERIKUTNYA
            LevelProgress.UnlockLevel(currentLevel + 1);
        }
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

    void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
