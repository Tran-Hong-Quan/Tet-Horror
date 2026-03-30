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

    private readonly float showRadius = 3f;
    private readonly Vector3 worldOffset = new(0f, 0f, 0f);
    private readonly float fadeSpeed = 3f;
    private readonly float maxAlpha = 1f;
    private readonly bool pulse = true;
    private readonly float pulseSpeed = 3f;
    private readonly float pulseMinAlpha = 0.4f;


    private void Awake()
    {
        rectTransform = transform as RectTransform;
        cam = Camera.main;

        iconImage = GetComponent<Image>();
        canvas = transform.parent.GetComponent<Canvas>();


        SetAlpha(0f);
    }

    private void LateUpdate()
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
        shouldShow = Vector3.Distance(PlayerInteract.Instance.transform.position, target.position) <= showRadius;
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