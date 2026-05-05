using UnityEngine;
using UnityEngine.Events;

public class PickableObject : InteractableObject
{
    [SerializeField] string objectNameKey;
    [SerializeField] Rigidbody rb;

    public UnityEvent<PickableObject> onPickUp;

    public Rigidbody Rigidbody => rb;
    public string ObjectNameKey => objectNameKey;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    protected override void OnInteract(CharacterInteract characterInteract)
    {
        base.OnInteract(characterInteract);
        PickUp(characterInteract);
    }

    public virtual void PickUp(CharacterInteract characterInteract)
    {
        SetPickable(false);
        characterInteract.PickUp(this);
        SetCanInspect(false);
        onPickUp?.Invoke(this);
    }

    public void SetPickable(bool isPickable)
    {
        if (rb != null)
        {
            rb.isKinematic = !isPickable;
            rb.detectCollisions = isPickable;
        }
    }

    public void Drop(CharacterInteract characterInteract)
    {
        transform.SetParent(null);

        if (rb != null)
        {
            SetPickable(true);
            rb.AddForce(characterInteract.transform.forward * characterInteract.ThrowHoldingObjectForce, ForceMode.Impulse);
        }

        SetCanInspect(true);
    }
}