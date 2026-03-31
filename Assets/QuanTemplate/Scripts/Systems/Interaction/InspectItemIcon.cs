using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InspectItemIcon : MonoBehaviour
{
    private Transform target;

    private Canvas canvas;
    private Image iconImage;
    private RectTransform rectTransform;
    private Camera cam;
    private float currentAlpha;
    private bool shouldShow;

    private Vector3 worldOffset = new(0f, 0f, 0f);
    private const float showRadius = 1.5f;
    private const float fadeSpeed = 3f;
    private const float maxAlpha = 1f;
    private const bool pulse = true;
    private const float pulseSpeed = 3f;
    private const float pulseMinAlpha = 0f;


    private void Awake()
    {
        rectTransform = transform as RectTransform;
        cam = Camera.main;

        iconImage = GetComponent<Image>();
        canvas = transform.parent.GetComponent<Canvas>();


        SetAlpha(0f);
    }

    private void Update()
    {
        if (cam == null)
            cam = Camera.main;

        if (target == null)
        {
            Destroy(gameObject);
        }

        UpdateTargetPosition();
        UpdateCheckRadius();
        UpdateFade();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetWorldOffset(Vector3 offset)
    {
        worldOffset = offset;
    }

    public void Show()
    {
        shouldShow = true;
    }

    public void Hide()
    {
        shouldShow = false;
    }

    private void UpdateCheckRadius()
    {
        if (PlayerInteract.Instance == null || !PlayerInteract.Instance.canControlPlayer)
        {
            shouldShow = false;
            return;
        }
        shouldShow = Vector3.Distance(cam.transform.position, target.position) <= showRadius;
    }

    private void UpdateTargetPosition()
    {
        if (target == null)
        {
            shouldShow = false;
            return;
        }

        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // Nếu object ở sau camera thì ẩn
        if (screenPos.z <= 0f)
        {
            iconImage.enabled = false;
            return;
        }

        iconImage.enabled = true;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam,
            out Vector2 localPoint
        );

        rectTransform.anchoredPosition = localPoint;
    }

    private void UpdateFade()
    {
        float targetAlpha = shouldShow ? maxAlpha : 0f;

        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);

        float finalAlpha = currentAlpha;

        if (shouldShow && pulse)
        {
            float pulseValue = Mathf.Lerp(
                pulseMinAlpha,
                1f,
                (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f
            );

            finalAlpha *= pulseValue;
        }

        SetAlpha(finalAlpha);
    }

    private void SetAlpha(float alpha)
    {
        if (iconImage == null)
            return;

        Color color = iconImage.color;
        color.a = alpha;
        iconImage.color = color;
    }
}