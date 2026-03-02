using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI textTimer;

    private float waktuBerjalan = 0f;
    private bool timerAktif = true;

    void Update()
    {
        if (!timerAktif) return;

        waktuBerjalan += Time.deltaTime;
        UpdateUI();
    }

    void UpdateUI()
    {
        int menit = Mathf.FloorToInt(waktuBerjalan / 60f);
        int detik = Mathf.FloorToInt(waktuBerjalan % 60f);

        textTimer.text = menit.ToString("00") + ":" + detik.ToString("00");
    }

    public void HentikanTimer()
    {
        timerAktif = false;
    }
}
