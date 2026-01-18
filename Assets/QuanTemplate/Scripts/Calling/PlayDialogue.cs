using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public DialogueData dialogueData;

    public UnityEvent<int> onDone;
    public void Play()
    {
        dialogueManager.StartDialogue(dialogueData, choice =>
        {
            onDone?.Invoke(choice);
        });
    }
}
