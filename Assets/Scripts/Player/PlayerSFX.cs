using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Clips")]
    public AudioClip walkClip;
    public AudioClip collectClip;
    public AudioClip hitClip;

    bool isWalking = false;

    public void PlayWalk()
    {
        if (isWalking) return;

        audioSource.clip = walkClip;
        audioSource.loop = true;
        audioSource.Play();
        isWalking = true;
    }

    public void StopWalk()
    {
        if (!isWalking) return;

        audioSource.Stop();
        isWalking = false;
    }

    public void PlayCollect()
    {
        audioSource.PlayOneShot(collectClip);
    }

    public void PlayHit()
    {
        audioSource.PlayOneShot(hitClip);
    }
}
