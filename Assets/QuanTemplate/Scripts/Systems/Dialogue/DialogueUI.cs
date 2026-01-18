using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    public GameObject rootUI;
    public Text speakerText;
    public Text dialogueText;
    public GameObject choicesContainer;
    public Button choiceButtonPrefab;
    public Button continueButton;
    public float textSpeed = 0.05f; // Thời gian giữa mỗi chữ xuất hiện
    public AudioSource typeSound; // Âm thanh khi gõ chữ

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentFullText;
    private List<string> choices;
    private DialogueManager manager;

    private Transform mainCamTransform;
    private Transform typeAudioSourceTransform;

    private void Start()
    {
        manager = GetComponent<DialogueManager>();
        manager.OnDialogueUpdated.AddListener(UpdateDialogueUI);
        continueButton.onClick.AddListener(NextDialogue);
        choicesContainer.SetActive(false);

        mainCamTransform = Camera.main.transform;
        typeAudioSourceTransform = typeSound.transform;
    }

    private void Update()
    {
        typeAudioSourceTransform.position = mainCamTransform.position;
    }

    private void UpdateDialogueUI(string speaker, string dialogue, List<string> choices)
    {
        rootUI.SetActive(true);
        continueButton.gameObject.SetActive(true);

        speakerText.text = speaker;
        currentFullText = dialogue;
        this.choices = choices;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeDialogue(currentFullText));

        foreach (Transform child in choicesContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in dialogue)
        {
            dialogueText.text += letter;
            if (typeSound) typeSound.PlayOneShot(typeSound.clip);
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        ShowChoices();
    }

    private void ShowChoices()
    {
        if (choices != null && choices.Count > 0)
        {
            choicesContainer.SetActive(true);
            for (int i = 0; i <choices.Count; i++)
            {
                var choice = choices[i];
                Button choiceButton = Instantiate(choiceButtonPrefab, choicesContainer.transform);
                choiceButton.GetComponentInChildren<Text>().text = choice;
                int t = i;
                choiceButton.onClick.AddListener(() => ChooseOption(t));
            }
            continueButton.gameObject.SetActive(false);
        }
        else
        {
            choicesContainer.SetActive(false);
        }
    }

    private void ChooseOption(int choiceIndex)
    {
        rootUI.SetActive(false);
        manager.NextDialogue(choiceIndex);
    }

    public void NextDialogue()
    {
        if (isTyping)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            dialogueText.text = currentFullText;
            isTyping = false;
            ShowChoices();
            return;
        }
        if (manager.IsLastDialogue()) rootUI.SetActive(false);
        manager.NextDialogue();
    }
}
