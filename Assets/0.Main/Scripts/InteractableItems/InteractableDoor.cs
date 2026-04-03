using UnityEngine;

public class InteractableDoor : InteractableObject
{
    [Header("Door Settings")]
    [SerializeField] Transform doorTransform;         // Cửa cần xoay
    [SerializeField] Vector3 closedEulerAngles;       // Góc khi đóng (Inspector)
    [SerializeField] Vector3 openEulerAngles;         // Góc khi mở (Inspector)
    [SerializeField] float rotateSpeed = 90f;         // Độ/giây xoay

    private bool isOpen = false;

    protected override void Start()
    {
        base.Start();
        if (doorTransform == null)
        {
            doorTransform = transform; 
        }
    }

    void Update()
    {
        // Luôn update xoay cửa theo trạng thái
        Quaternion targetRotation = isOpen
            ? Quaternion.Euler(openEulerAngles)
            : Quaternion.Euler(closedEulerAngles);

        // Xoay cửa đi theo đường ngắn nhất, mượt
        doorTransform.rotation = Quaternion.RotateTowards(
            doorTransform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }
    public override void Interact(CharacterInteract characterInteract)
    {
        base.Interact(characterInteract);
        OpenOrCloseDoor();
    }

    public void OpenOrCloseDoor()
    {
        isOpen = !isOpen;
    }
}
