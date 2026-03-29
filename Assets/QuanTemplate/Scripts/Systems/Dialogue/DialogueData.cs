using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class DialogueLine
{
    [SerializeField] private LocalizedString speakerName;
    [SerializeField] private LocalizedString dialogueText;
    
    public string GetSpeakerName()
    {
        return speakerName.GetLocalizedString();
    }

    public string GetDialogueText()
    {
        return dialogueText.GetLocalizedString();
    }
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines; 
    public List<string> choices;  
}
