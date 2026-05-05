using UnityEngine;
using UnityEngine.Localization;

public class InspectableObject : MonoBehaviour
{
    [SerializeField] LocalizedString localizedMessage;
    [SerializeField] Transform iconTarget;
    [SerializeField] Vector3 worldIconOffset = Vector3.zero;

    public bool CanInspect { get; private set; } = true;
    private string message;
    protected InspectItemIcon inspectItemIcon;

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
        CanInspect = true;
        InitIcon();
    }

    protected virtual void OnDestroy()
    {
        if (!localizedMessage.IsEmpty)
        {
            localizedMessage.StringChanged -= OnMessageChanged;
        }
        if (inspectItemIcon != null)
        {
            Destroy(inspectItemIcon.gameObject);
        }
    }

    private void OnEnable()
    {
        if (inspectItemIcon != null)
        {
            inspectItemIcon.gameObject.SetActive(true);
        }
    }

    protected void OnDisable()
    {
        if (inspectItemIcon != null)
        {
            inspectItemIcon.gameObject.SetActive(false);
        }
    }

    public void SetCanInspect(bool canInspect)
    {
        if (inspectItemIcon != null)
        {
            inspectItemIcon.gameObject.SetActive(canInspect);
        }
        this.CanInspect = canInspect;
    }

    private void OnMessageChanged(string message)
    {
        this.message = message;
    }

    public virtual string GetMessage(CharacterInteract characterInteract)
    {
        if (!CanInspect)
        {
            return "";
        }
        return message;
    }
    private void InitIcon()
    {
        if (GetShowInspactItemIcon() == false)
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
        return PlayerPrefs.GetInt(ShowInspectItemIconKey, 1) == 1;
    }
}
