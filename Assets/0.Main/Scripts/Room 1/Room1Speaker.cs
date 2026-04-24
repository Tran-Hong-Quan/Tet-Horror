using UnityEngine;

public class Room1Speaker : InteractableObject
{
    public bool isMusicOn = true;
    public AudioSource audioSource;

    public override void Interact(CharacterInteract characterInteract)
    {
        base.Interact(characterInteract);
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
