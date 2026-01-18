using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningObject : InteractableObject
{
    public string warningMessageKey;
    public override void Interact(CharacterInteract characterInteract)
    {
        base.Interact(characterInteract);
        if(characterInteract is PlayerInteract)
        {
            var player = characterInteract as PlayerInteract;
            player.ShowWarning(warningMessageKey);
        }
    }
}
