using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource audioSource;

    public void ChangeAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
