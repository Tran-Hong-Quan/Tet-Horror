using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;  // Tên người 
    [TextArea(2, 5)] public string dialogueText;  // Nội dung hội thoại
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines;  // Danh sách các câu thoại
    public List<string> choices;  
}
