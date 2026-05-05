using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableOpenCanvasObject : InteractableObject
{
    [SerializeField] GameObject canvas;

    public UnityEvent<InteractableObject> onInteract;

    PlayerInteract playerInteract;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnInteract(CharacterInteract characterInteract)
    {
        base.OnInteract(characterInteract);
        if (characterInteract is not PlayerInteract)
        {
            return;
        }
        playerInteract = (PlayerInteract)characterInteract;
        canvas.SetActive(true);
        playerInteract.DisableControlPlayer();

        onInteract?.Invoke(this);
    }

    public void CloseCanvas()
    {
        canvas.SetActive(false);
        if (playerInteract != null)
        {
            playerInteract.EnableControlPlayer();
        }
    }
}
