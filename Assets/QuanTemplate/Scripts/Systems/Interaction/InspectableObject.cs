using UnityEngine;
using UnityEngine.Localization;

public class InspectableObject : MonoBehaviour
{
    [SerializeField] LocalizedString localizedMessage;
    [SerializeField] Transform iconTarget;
    [SerializeField] Vector3 worldIconOffset = Vector3.zero;

    private string message;
    private InspectItemIcon inspectItemIcon;

    const string ResourceIconPath = "InspectItemIcon";
    const string ShowInspectItemIconKey = "ShowInspectItemIcon";

    protected virtual void Start()
    {
        if (!localizedMessage.IsEmpty)
        {
            localizedMessage.StringChanged += OnMessageChanged;
            localizedMessage.RefreshString();
        }

        iconTarget = iconTarget != null ? iconTarget : transform;

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
        if(GetShowInspactItemIcon() == false)
        {
            return;
        }
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
        inspectItemIcon.SetTarget(iconTarget);
        inspectItemIcon.SetWorldOffset(worldIconOffset);
    }

    public static void SetShowInspactItemIcon(bool isShow)
    {
        PlayerPrefs.SetInt(ShowInspectItemIconKey, isShow ? 1 : 0);
    }

    public static bool GetShowInspactItemIcon()
    {
        return PlayerPrefs.GetInt(ShowInspectItemIconKey, 0) == 1;
    }
}
