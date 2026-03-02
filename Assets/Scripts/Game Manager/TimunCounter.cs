using UnityEngine;
using TMPro;

public class TimunCounter : MonoBehaviour
{
    public static TimunCounter instance;
    public int totalTimun = 0;
    public int timunTerkumpul = 0;
    public TextMeshProUGUI textTimun;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        totalTimun = GameObject.FindGameObjectsWithTag("Timun").Length;
        UpdateUI();
        Debug.Log("Total Timun di Level: " + totalTimun);
    }

    public void TambahTimun()
    {
        timunTerkumpul++;
        Debug.Log("Timun terkumpul: " + timunTerkumpul + " / " + totalTimun);
        UpdateUI();
    }

    void UpdateUI()
    {
        textTimun.text = timunTerkumpul + " / " + totalTimun;
    }
}