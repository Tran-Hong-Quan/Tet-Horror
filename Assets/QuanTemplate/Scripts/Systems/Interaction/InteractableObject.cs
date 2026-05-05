public class InteractableObject : InspectableObject
{
    public virtual void Interact(CharacterInteract characterInteract)
    {
        if (!CanInspect)
        {
            return;
        }
        OnInteract(characterInteract);
    }

    protected virtual void OnInteract(CharacterInteract characterInteract) { }
}
