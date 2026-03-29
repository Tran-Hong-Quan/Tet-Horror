using UnityEngine;
using UnityEngine.Localization;

public class InspectableObject : MonoBehaviour
{
    public LocalizedString localizedMessage;

    private string message;

    protected virtual void Start()
    {
        if (localizedMessage.IsEmpty)
        {
            return;
        }
        localizedMessage.StringChanged += OnMessageChanged;
        localizedMessage.RefreshString();
    }

    protected virtual void OnDestroy()
    {
        if (localizedMessage.IsEmpty)
        {
            return;
        }
        localizedMessage.StringChanged -= OnMessageChanged;
    }

    private void OnMessageChanged(string message)
    {
        this.message = message;
    }

    public virtual string GetMessage(CharacterInteract characterInteract)
    {
        return message;
    }
}
