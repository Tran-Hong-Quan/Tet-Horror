using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewableObject : InteractableObject
{
    [SerializeField] Transform viewModel;

    public Transform ViewModel => viewModel;

    public override void Interact(CharacterInteract characterInteract)
    {
        base.Interact(characterInteract);
        View(characterInteract);
    }

    public virtual void View(CharacterInteract characterInteract)
    {
        if (!(characterInteract as PlayerInteract)) return;
        var playerInteract = (PlayerInteract)characterInteract;
        playerInteract.ViewObject(this);
    }
}
