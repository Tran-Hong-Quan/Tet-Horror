using UnityEngine;

public class Room1Speaker : InteractableObject
{
    public bool isMusicOn = true;
    public AudioSource audioSource;

    protected override void OnInteract(CharacterInteract characterInteract)
    {
        base.OnInteract(characterInteract);
        ToggleMusic();
    }

    public void ToggleMusic()
    {
        if (audioSource != null)
        {
            isMusicOn = !isMusicOn;
            if (isMusicOn)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Pause();
            }
        }
    }
}
