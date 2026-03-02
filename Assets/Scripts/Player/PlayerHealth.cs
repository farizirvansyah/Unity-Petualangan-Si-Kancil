using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 3;
    public int currentHP;
    public ResultUI resultUI;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        
        PlayerSFX sfx = GetComponent<PlayerSFX>();
        if (sfx != null)
        {
            sfx.PlayHit();
        }

        Debug.Log("HP Player: " + currentHP);

        if (currentHP <= 0)
        {
            Mati();
            Debug.Log("Player Mati");
        }
    }

    void Mati()
    {
        Debug.Log("Player Mati");
        resultUI.TampilkanResult(0);
        Time.timeScale = 0f;
    }
}
