using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningObject : InteractableObject
{
    public string warningMessageKey;
    protected override void OnInteract(CharacterInteract characterInteract)
    {
        base.OnInteract(characterInteract);
        if(characterInteract is PlayerInteract)
        {
            var player = characterInteract as PlayerInteract;
            player.ShowWarning(warningMessageKey);
        }
    }
}
