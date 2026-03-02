using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public int health = 3;
    public int numOfHearts = 3;
    public Image[] hearts;
    public Image[] heartsResult;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public PlayerHealth playerHealth;
    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
                heartsResult[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
                heartsResult[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
                heartsResult[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
                heartsResult[i].enabled = false;
            }
        }
        health = playerHealth.currentHP;
    }
}
