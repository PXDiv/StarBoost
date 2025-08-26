using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volume);
    }
}
