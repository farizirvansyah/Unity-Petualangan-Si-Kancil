using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("--------------------- Pop Up Quit")]
    [SerializeField] private GameObject quitPanel; // Panel untuk konfirmasi keluar
    [SerializeField] private Toggle toggleNarrativeSkip; // Toggle untuk mengaktifkan/menonaktifkan Narrative Skip

    private void Start()
    {
        // Inisialisasi status toggle berdasarkan pengaturan yang disimpan
        if (toggleNarrativeSkip != null)
        {
            toggleNarrativeSkip.isOn = NarrativeSettings.IsSkipEnabled();
            toggleNarrativeSkip.onValueChanged.AddListener(OnToggleNarrativeSkipChanged);
        }
    }
    
    void Update()
    {
        // Tangani tombol Escape
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleEscapeInput();
        }
    }

    private void OnToggleNarrativeSkipChanged(bool isOn)
    {
        NarrativeSettings.SetSkip(isOn);
        Debug.Log("Narrative Skip diubah: " + isOn);
    }

    private void HandleEscapeInput()
    {
        // Jika berada di Main Menu
        if (quitPanel != null && !quitPanel.activeSelf) // Tampilkan panel Quit
        {
            quitPanel.SetActive(true);
            Debug.Log("Quit Panel Aktif");
        }
        else if (quitPanel != null && quitPanel.activeSelf) // Sembunyikan panel Quit
        {
            quitPanel.SetActive(false);
            Debug.Log("Quit Panel Non-Aktif");
        }
    }

    public void onUserChangeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene); // Ganti Scene
        Debug.Log("Change Scene");
    }
    public void onUserQuitConfirm()
    {
        Application.Quit(); // Keluar game
        Debug.Log("Quit Game");
    }

    public void onUserQuitCancel()
    {
        if (quitPanel != null) quitPanel.SetActive(false); // Sembunyikan panel Quit
        Debug.Log("Cancel Quit");
    }
}