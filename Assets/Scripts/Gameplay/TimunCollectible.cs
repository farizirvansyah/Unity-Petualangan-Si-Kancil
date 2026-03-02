using UnityEngine;

public class TimunCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSFX sfx = other.GetComponent<PlayerSFX>();
            sfx.PlayCollect();
            TimunCounter.instance.TambahTimun();
            PlayerSpeedBuff speedBuff =
                other.GetComponent<PlayerSpeedBuff>();

            if (speedBuff != null)
            {
                speedBuff.ActivateSpeedBuff();
            }

            Destroy(gameObject);
        }
    }
}
