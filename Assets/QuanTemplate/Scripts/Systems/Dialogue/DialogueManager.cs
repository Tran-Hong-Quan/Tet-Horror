using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public UnityEvent<string, string, List<string>> OnDialogueUpdated; // Gọi UI khi hội thoại thay đổi
    private Queue<DialogueLine> dialogueQueue;
    private DialogueData dialogueData;
    private System.Action<int> onDoneDialogue = null;
    private bool isDialogueActive = false;

    private void Awake()
    {
        dialogueQueue = new Queue<DialogueLine>();
    }

    public void StartDialogue(DialogueData dialogue, System.Action<int> onDoneDialogue = null)
    {
        if (isDialogueActive) return;
        isDialogueActive = true;

        dialogueData = dialogue;
        this.onDoneDialogue = onDoneDialogue;

        dialogueQueue.Clear();
        foreach (var line in dialogue.lines)
        {
            dialogueQueue.Enqueue(line);
        }

        NextDialogue();
    }

    public void NextDialogue(int choice = -1)
    {
        if (IsLastDialogue())
        {
            ///print("End Dialgu");
            EndDialogue(choice);
            return;
        }

        var currentLine = dialogueQueue.Dequeue();
        List<string> choices = null;
        if (IsLastDialogue())
            choices = dialogueData.choices;
        OnDialogueUpdated?.Invoke(currentLine.speakerName, currentLine.dialogueText, choices);
    }
    public bool IsLastDialogue()
    {
        return dialogueQueue.Count == 0;
    }

    public void EndDialogue(int choice = -1)
    {
        isDialogueActive = false;
        onDoneDialogue?.Invoke(choice);
    }
}
