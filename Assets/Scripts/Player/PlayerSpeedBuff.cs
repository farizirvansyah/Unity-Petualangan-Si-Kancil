using UnityEngine;
using System.Collections;

public class PlayerSpeedBuff : MonoBehaviour
{
    public float normalSpeed = 3f;
    public float buffSpeed = 5f;
    public float buffDuration = 3f;

    PlayerController movement;
    Coroutine buffCoroutine;

    void Start()
    {
        movement = GetComponent<PlayerController>();
        normalSpeed = movement.moveSpeed;
    }

    public void ActivateSpeedBuff()
    {
        // Jika buff masih aktif → reset durasi
        if (buffCoroutine != null)
        {
            StopCoroutine(buffCoroutine);
        }

        buffCoroutine = StartCoroutine(SpeedBuffRoutine());
    }

    IEnumerator SpeedBuffRoutine()
    {
        movement.moveSpeed = buffSpeed;
        Debug.Log("Speed Buff Aktif");

        yield return new WaitForSeconds(buffDuration);

        movement.moveSpeed = normalSpeed;
        Debug.Log("Speed Buff Berakhir");

        buffCoroutine = null;
    }
}
