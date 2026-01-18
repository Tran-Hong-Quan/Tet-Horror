using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public string openKey = "Open";
    public string closeKey = "Close";

    public Vector3 openAngle = Vector3.zero;
    public Vector3 closeAngle = Vector3.zero;

    public float duration = .7f;
    public bool isOpen = false;

    public AudioClip openSound;
    public AudioClip closeSound;

    private Tween tween;
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override string GetMessage(CharacterInteract characterInteract)
    {
        return isOpen ? closeKey : openKey;
    }

    public override void Interact(CharacterInteract characterInteract)
    {
        base.Interact(characterInteract);
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        tween?.Kill();
        tween = transform.DOLocalRotateQuaternion(Quaternion.Euler(isOpen ? closeAngle : openAngle), duration);
        if (isOpen)
        {
            audioSource.clip = closeSound;
        }
        else
        {
            audioSource.clip = openSound;
        }
        audioSource.Play();
        isOpen = !isOpen;
    }
}
