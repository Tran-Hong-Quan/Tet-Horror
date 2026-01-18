using UnityEngine;

public class InspectableObject : MonoBehaviour
{
    public string messageKey; 
    public bool isCommonMessage = true;

    public virtual string GetMessage(CharacterInteract characterInteract)
    {
        return messageKey;  
    }
}
