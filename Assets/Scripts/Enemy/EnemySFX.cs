using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySFX : MonoBehaviour
{
    public AudioSource audioSource;
    [Header("SFX")]
    public AudioClip patrolSFX;
    public AudioClip chaseSFX;
    public AudioClip attackSFX;

    EnemyChaseGroundAStar.EnemyState lastState;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    public void OnStateChanged(EnemyChaseGroundAStar.EnemyState newState)
    {
        if (newState == lastState) return;
        lastState = newState;

        switch (newState)
        {
            case EnemyChaseGroundAStar.EnemyState.Patrol:
                PlayLoop(patrolSFX, 0.25f);
                break;

            case EnemyChaseGroundAStar.EnemyState.Chase:
                PlayLoop(chaseSFX, 0.4f);
                break;

            case EnemyChaseGroundAStar.EnemyState.Attack:
                PlayOneShot(attackSFX);
                break;
        }
    }

    void PlayLoop(AudioClip clip, float volume)
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    void PlayOneShot(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }
}