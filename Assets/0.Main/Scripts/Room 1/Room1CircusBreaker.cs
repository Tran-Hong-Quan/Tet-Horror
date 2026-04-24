using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class Room1CircusBreaker : InteractableObject
{
    [SerializeField] Transform lid;
    [SerializeField] Vector3 openOffsetRotation = new Vector3(-180, 0, 0);


    [SerializeField] float openLidDuration = 0.5f;
    [SerializeField] GameObject openTool;
    [SerializeField] LocalizedString warningText;

    [SerializeField] Material breakerMaterial;
    [SerializeField] Color turnOnColor = Color.green;
    [SerializeField] Color turnOffColor = Color.red;

    [SerializeField] GameObject electricShockEffect;

    public bool canTurnOnCircusBreaker = false;
    public UnityEvent onTurnOnCircusBreaker;

    bool isOpen = false;
    bool isTurnOn = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    protected override void Start()
    {
        base.Start();
        closedRotation = lid.rotation;
        openRotation = closedRotation * Quaternion.Euler(openOffsetRotation);
        isTurnOn = true;
        if (breakerMaterial != null)
        {
            breakerMaterial.color = turnOnColor;
        }
    }

    public override void Interact(CharacterInteract characterInteract)
    {
        base.Interact(characterInteract);
        if (!isOpen)
        {
            TryOpenLid(characterInteract);
        }
        else if (!isTurnOn)
        {
            TurnOn();
        }
    }

    void TryOpenLid(CharacterInteract characterInteract)
    {
        if (characterInteract.HoldingObject == null || characterInteract.HoldingObject.gameObject != openTool)
        {
            if (characterInteract is PlayerInteract playerInteract)
            {
                playerInteract.ShowWarning(warningText.GetLocalizedString());
            }
            return;
        }

        isOpen = true;
        lid.DORotateQuaternion(openRotation, openLidDuration);
    }

    void TurnOn()
    {
        if(!canTurnOnCircusBreaker)
        {
            return;
        }
        isTurnOn = true;
        if (breakerMaterial != null)
        {
            breakerMaterial.color = turnOnColor;
        }
        onTurnOnCircusBreaker?.Invoke();
    }

    public void TurnOff()
    {
        isTurnOn = false;
        if (breakerMaterial != null)
        {
            breakerMaterial.color = turnOffColor;
        }
    }

    public void ElectricShock()
    {
        canTurnOnCircusBreaker = true;
        TurnOff();
        electricShockEffect.SetActive(true);

    }
}
