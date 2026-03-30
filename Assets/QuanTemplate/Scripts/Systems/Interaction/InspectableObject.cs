using UnityEngine;
using UnityEngine.Localization;

public class InspectableObject : MonoBehaviour
{
    [SerializeField] LocalizedString localizedMessage;
    [SerializeField] Transform targetObject;

    private string message;
    private InspectItemIcon inspectItemIcon;

    private const string ResourceIconPath = "InspectItemIcon";

    protected virtual void Start()
    {
        if (!localizedMessage.IsEmpty)
        {
            localizedMessage.StringChanged += OnMessageChanged;
            localizedMessage.RefreshString();
        }

        targetObject = targetObject != null ? targetObject : transform;

        InitIcon();
    }

    protected virtual void OnDestroy()
    {
        if (!localizedMessage.IsEmpty)
        {
            localizedMessage.StringChanged -= OnMessageChanged;
        }
    }

    private void OnMessageChanged(string message)
    {
        this.message = message;
    }

    public virtual string GetMessage(CharacterInteract characterInteract)
    {
        return message;
    }
    private void InitIcon()
    {
        InspectItemCanvas inspectItemCanvas = InspectItemCanvas.Get();
        if (inspectItemCanvas == null)
        {
            Debug.LogError("Could not get InspectItemCanvas in the scene.");
            return;
        }
        var prefab = Resources.Load<InspectItemIcon>(ResourceIconPath);
        if (prefab == null)
        {
            Debug.LogError($"Could not find prefab at Resources/{ResourceIconPath}");
            return;
        }
        inspectItemIcon = Instantiate(prefab, inspectItemCanvas.transform, true);
        inspectItemIcon.SetTarget(transform);
    }
}
