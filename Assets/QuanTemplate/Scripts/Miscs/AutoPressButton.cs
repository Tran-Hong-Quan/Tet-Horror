using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AutoPressButton : MonoBehaviour
{
    public Button targetButton; // Kéo nút UI vào đây
    public InputActionReference actionTrigger; // Gán Input Action cần kích hoạt nút

    private void Awake()
    {
        var navi = new Navigation
        {
            mode = Navigation.Mode.None
        };
        targetButton.navigation = navi;
    }

    private void OnEnable()
    {
        if (actionTrigger != null)
        {
            actionTrigger.action.performed += OnActionTriggered;
            actionTrigger.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (actionTrigger != null)
        {
            actionTrigger.action.performed -= OnActionTriggered;
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (targetButton != null)
        {
            targetButton.onClick.Invoke(); // Nhấn nút UI khi action được trigger
        }
    }
}
