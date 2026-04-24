using UnityEngine;
using UnityEngine.Events;

public class Room2Manager : MonoBehaviour
{
    [SerializeField] private float delayBeforeStart = 1f;
    [SerializeField] private DialogueData introDialogue1;
    [SerializeField] private DialogueData introDialogue2;
    [SerializeField] private DialogueData introDialogue3;

    public UnityEvent onDoneIntroDialogue1;
    public UnityEvent onDoneIntroDialogue2;
    public UnityEvent onDoneIntroDialogue3;

    private void Start()
    {
        PlayerInteract.Instance.DisableControlPlayer();
        this.DelayFunction(delayBeforeStart, PlayIntroDialogue1);
    }

    public void PlayIntroDialogue1()
    {
        PlayerInteract.Instance.Dialogue(introDialogue1, _ =>
        {
            onDoneIntroDialogue1.Invoke();
        }, false);
    }
    public void PlayIntroDialogue2()
    {
        PlayerInteract.Instance.Dialogue(introDialogue2, _ =>
        {
            onDoneIntroDialogue2.Invoke();
        }, false);
    }
    public void PlayIntroDialogue3()
    {
        PlayerInteract.Instance.Dialogue(introDialogue3, _ =>
        {
            onDoneIntroDialogue3.Invoke();
        }, false);
    }
}
