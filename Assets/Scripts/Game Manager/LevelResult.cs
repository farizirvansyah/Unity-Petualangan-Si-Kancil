using UnityEngine;

public class LevelResult : MonoBehaviour
{
    public static LevelResult instance;

    public EpilogController epilogController;
    public LevelTimer levelTimer;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void HitungBintang()
    {
        levelTimer.HentikanTimer();

        int terkumpul = TimunCounter.instance.timunTerkumpul;
        int total = TimunCounter.instance.totalTimun;

        float persen = (float)terkumpul / total * 100f;
        int bintang;

        if (persen == 100f) bintang = 3;
        else if (persen > 50f) bintang = 2;
        else bintang = 1;

        epilogController.ShowEpilog(bintang);
        Debug.Log("Level selesai dengan " + bintang + " bintang");
    }
}
