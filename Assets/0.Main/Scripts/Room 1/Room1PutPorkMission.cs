using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Room1PutPorkMission : InteractableObject
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Transform targetRoot;
    [SerializeField] private float moveDuration = 1f;

    [SerializeField] private Material blinkMaterial;
    [SerializeField] private float blinkMinAlpha = 0.2f;
    [SerializeField] private float blinkMaxAlpha = 1f;
    [SerializeField] private float blinkSpeed = 2f;

    [SerializeField] Transform targetLixiTrans;
    [SerializeField] Transform lixi;


    private Color originalColor;

    [SerializeField] UnityEvent onMissionComplete;
    bool isMissionComplete = false;

    protected override void Start()
    {
        base.Start();
        if (blinkMaterial != null)
        {
            originalColor = blinkMaterial.color;
        }
    }

    private void Update()
    {
        if (blinkMaterial == null) return;

        float alpha = Mathf.Lerp(
            blinkMinAlpha,
            blinkMaxAlpha,
            (Mathf.Sin(Time.time * blinkSpeed) + 1f) * 0.5f
        );

        Color color = originalColor;
        color.a = alpha;
        blinkMaterial.color = color;
    }

    private void OnDisable()
    {
        if (blinkMaterial != null)
        {
            blinkMaterial.color = originalColor;
        }
    }

    public override void Interact(CharacterInteract characterInteract)
    {
        base.Interact(characterInteract);

        if (!isMissionComplete && characterInteract.HoldingObject != null && characterInteract.HoldingObject.gameObject == targetObject)
        {
            characterInteract.DropHoldingObject();
            targetObject.GetComponent<Rigidbody>().isKinematic = true;
            targetObject.GetComponent<Collider>().enabled = false;
            targetObject.transform.SetParent(targetRoot, true);
            targetObject.transform.DOLocalMove(Vector3.zero, moveDuration).SetEase(Ease.InOutSine);
            targetObject.transform.DOLocalRotate(Vector3.zero, moveDuration).SetEase(Ease.InOutSine);
            lixi.transform.SetParent(targetLixiTrans, true);
            lixi.transform.DOLocalMove(Vector3.zero, moveDuration).SetEase(Ease.InOutSine);
            lixi.transform.DOLocalRotate(Vector3.zero, moveDuration).SetEase(Ease.InOutSine);
            isMissionComplete = true;
            onMissionComplete.Invoke();
        }
    }
}

