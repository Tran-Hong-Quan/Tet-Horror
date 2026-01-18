using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#if UNITY_STANDALONE || UNITY_WEBGL
using UnityEngine.InputSystem;
#endif

public class ViewableCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 5f;
    private Vector2 lastInputPosition;
    private Transform viewObject;

    private bool delayRotate = false;

#if UNITY_ANDROID || UNITY_IOS
    private bool isDragging;
#endif

    private void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Enable(); // Chỉ bật cảm ứng trên Mobile
#endif
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        HandleMouseInput();  // Chỉ chạy trên PC
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();  // Chỉ chạy trên Mobile
#endif
    }

#if UNITY_STANDALONE || UNITY_WEBGL
    private void HandleMouseInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || delayRotate)
        {
            lastInputPosition = Mouse.current.position.ReadValue();
            return;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 delta = mousePosition - lastInputPosition;
            lastInputPosition = mousePosition;

            RotateObject(delta);
        }
    }
#endif

#if UNITY_ANDROID || UNITY_IOS
    private void HandleTouchInput()
    {
        if (Touch.activeTouches.Count == 1)
        {
            Touch touch = Touch.activeTouches[0];
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began || delayRotate)
            {
                isDragging = true;
                lastInputPosition = touch.screenPosition;
            }
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.screenPosition - lastInputPosition;
                lastInputPosition = touch.screenPosition;

                RotateObject(delta);
            }
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }
#endif

    private void RotateObject(Vector2 delta)
    {
        float rotationX = delta.y * rotationSpeed * Time.deltaTime;
        float rotationY = -delta.x * rotationSpeed * Time.deltaTime;

        target.Rotate(Vector3.right, rotationX, Space.World);
        target.Rotate(Vector3.up, rotationY, Space.World);
    }

    public void SetViewObject(Transform viewObject)
    {
        gameObject.SetActive(true);
        target.localRotation = Quaternion.Euler(Vector3.zero);
        delayRotate = true;
        this.DelayFunction(.2f,() => delayRotate = false);
        if (this.viewObject) Destroy(this.viewObject.gameObject);
        this.viewObject = viewObject;
        viewObject.SetParent(target, false);
        viewObject.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        viewObject.localScale = Vector3.one;
    }
}
