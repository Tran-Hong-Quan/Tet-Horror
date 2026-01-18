using DG.Tweening;
using QFSW.QC;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : CharacterInteract
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] Text inspectText;

    [SerializeField] CanvasGroup warningTextRoot;
    [SerializeField] Text warningText;
    [SerializeField] float warningTextShowDuration = 1;
    [SerializeField] float warningTextHideDuration = .5f;

    [SerializeField] ViewableCamera viewableCamera;
    [SerializeField] GameObject viewObjectUI;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] FollowTransform rigLookTarget;

    [SerializeField] GameObject leftClickUIButtonn;
    [SerializeField] GameObject rightClickUIButtonn;

    private StarterAssetsInputs inputs;
    private InspectableObject inspectableObject;
    private ViewableObject viewableObject;
    private FirstPersonController firstPersonController;
    [HideInInspector] public bool canControlPlayer = true;
    private Tween warningTween;
    private bool lastControlPlayer;


    public FirstPersonController FirstPersonController => firstPersonController;

    protected override void Awake()
    {
        base.Awake();
        inputs = GetComponent<StarterAssetsInputs>();
        firstPersonController = GetComponent<FirstPersonController>();
    }

    private void Start()
    {
        if (QuantumConsole.Instance)
        {
            QuantumConsole.Instance.OnActivate += OnOpenConsole;
            QuantumConsole.Instance.OnDeactivate += OnCloseConsole;
            if (QuantumConsole.Instance.IsActive) OnOpenConsole();
        }
    }

    private void OnDestroy()
    {
        if (QuantumConsole.Instance)
        {
            QuantumConsole.Instance.OnActivate -= OnOpenConsole;
            QuantumConsole.Instance.OnDeactivate -= OnCloseConsole;
        }
    }

    private void Update()
    {
        if (!canControlPlayer) return;

        Ray ray = new(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            InspectableObject inspectable = null;

            if (hit.collider.CompareTag(Tags.InspectableObject))
            {
                inspectable = hit.collider.GetComponent<InspectableObject>();
                if (inspectable == null)
                    inspectable = hit.transform.GetComponentInParent<InspectableObject>();
            }

            if (inspectable != null && inspectable.GetMessage(this) != "")
            {
                inspectText.text = inspectable.GetMessage(this);
                inspectText.enabled = true;
                inspectableObject = inspectable;
            }
            else
            {
                inspectableObject = null;
                inspectText.enabled = false;
            }
        }
        else
        {
            inspectableObject = null;
            inspectText.enabled = false;
        }

        if (inputs.primary)
        {
            inputs.primary = false;
            if (inspectableObject != null && inspectableObject is InteractableObject)
            {
                (inspectableObject as InteractableObject).Interact(this);
            }
        }

        if (inputs.dropHoldingObject)
        {
            inputs.dropHoldingObject = false;
            DropHoldingObject();
        }
#if UNITY_ANDROID || UNITY_ANDROID
#endif
        leftClickUIButtonn.SetActive(inspectableObject != null && inspectableObject is InteractableObject);
        rightClickUIButtonn.SetActive(holdingObject != null);
    }


    public void ViewObject(ViewableObject viewableObject)
    {
        if (this.viewableObject != null) return;
        this.viewableObject = viewableObject;
        viewableCamera.SetViewObject(Instantiate(viewableObject.ViewModel));
        viewObjectUI.SetActive(true);

        DisableControlPlayer();
    }

    public void OffViewObject()
    {
        viewableObject = null;
        viewableCamera.gameObject.SetActive(false);
        viewObjectUI.SetActive(false);

        EnableControlPlayer();
    }

    public void Dialogue(DialogueData dialogueData, System.Action<int> onDoneDialogue = null)
    {
        var t = onDoneDialogue;
        onDoneDialogue = DoneDialogue;
        onDoneDialogue += t;

        DisableControlPlayer();

        dialogueManager.StartDialogue(dialogueData, onDoneDialogue);
    }

    private void DoneDialogue(int choiceID)
    {
        EnableControlPlayer();
    }

    //Tween lookRigTween;
    public void EnableControlPlayer()
    {
        canControlPlayer = true;
        firstPersonController.SetCanMove(true);
        firstPersonController.SetActivePlayerUI(true);
        inputs.SetCursorState(inputs.cursorLocked);
        inputs.primary = false;

        //rigLookTarget.follow = true;
    }
    public void DisableControlPlayer()
    {
        canControlPlayer = false;
        firstPersonController.SetCanMove(false);
        firstPersonController.SetActivePlayerUI(false);
        inputs.SetCursorState(false);

        //rigLookTarget.follow = false;
    }

    public void ShowWarning(string message)
    {
        warningTween?.Kill();
        warningTextRoot.gameObject.SetActive(true);
        warningText.text = message;
        warningTextRoot.alpha = 1f;
        warningTween = warningTextRoot.DOFade(0, warningTextHideDuration).SetDelay(warningTextShowDuration).OnComplete(() => warningTextRoot.gameObject.SetActive(false));
    }

    private void OnOpenConsole()
    {
        lastControlPlayer = canControlPlayer;
        DisableControlPlayer();
    }

    private void OnCloseConsole()
    {
        if (lastControlPlayer)
        {
            EnableControlPlayer();
        }
        else
        {
            DisableControlPlayer();
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCamera == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCamera.position, playerCamera.position + playerCamera.forward * maxDistance);
    }
}
