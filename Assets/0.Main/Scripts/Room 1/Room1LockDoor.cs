using UnityEngine;
using UnityEngine.UI;

public class Room1LockDoor : InteractableDoor
{
    [Header("UI")]
    [SerializeField] GameObject lockPasswordCanvas;
    [SerializeField] Text displayText;
    [SerializeField] Button[] buttons;

    [Header("Settings")]
    [SerializeField] string correctPassword = "1234";
    [SerializeField] bool autoCheckWhenFull = true; // tự check khi nhập đủ

    private string currentInput = "";
    private int maxLength;

    private PlayerInteract playerInteract;
    bool isDoorUnlocked = false;

    protected override void Start()
    {
        base.Start();

        maxLength = correctPassword.Length; // 🔥 lấy độ dài ở đây

        if (!isDoorUnlocked)
        {
            foreach (var button in buttons)
            {
                string key = button.gameObject.name;
                button.onClick.AddListener(() => OnButtonPressed(key));
            }
            UpdateDisplay();
        }
    }

    protected override void OnInteract(CharacterInteract characterInteract)
    {
        if (!isDoorUnlocked)
        {
            if (characterInteract is PlayerInteract playerInteract)
            {
                ShowLockPasswordCanvas(playerInteract);
            }
        }
        else
        {
            OpenOrCloseDoor();
        }
    }

    void ShowLockPasswordCanvas(PlayerInteract playerInteract)
    {
        lockPasswordCanvas.SetActive(true);
        playerInteract.DisableControlPlayer();
        this.playerInteract = playerInteract;
    }

    public void CloseLockPasswordCanvas()
    {
        lockPasswordCanvas.SetActive(false);
        this.playerInteract.EnableControlPlayer();
    }

    private enum DisplayState
    {
        Input,
        Wrong,
        Unlock
    }

    private DisplayState displayState = DisplayState.Input;

    public void OnButtonPressed(string key)
    {
        if (displayState == DisplayState.Unlock)
            return;

        if (key == "*")
        {
            currentInput = "";

            if (displayState == DisplayState.Wrong)
                displayState = DisplayState.Input;
        }
        else if (key == "#")
        {
            CheckPassword();
            return;
        }
        else
        {
            if (displayState == DisplayState.Wrong)
                displayState = DisplayState.Input;

            // 🔥 GIỚI HẠN ĐỘ DÀI
            if (currentInput.Length >= maxLength)
                return;

            currentInput += key;

            // 🔥 AUTO CHECK
            if (autoCheckWhenFull && currentInput.Length == maxLength)
            {
                CheckPassword();
                return;
            }
        }

        UpdateDisplay();
    }

    private void CheckPassword()
    {
        if (currentInput == correctPassword)
        {
            displayState = DisplayState.Unlock;
            displayText.text = "UNLOCK";

            isDoorUnlocked = true;
            OpenOrCloseDoor();
        }
        else
        {
            displayState = DisplayState.Wrong;
            displayText.text = "WRONG";
            currentInput = "";
        }
    }

    private void UpdateDisplay()
    {
        if (displayState == DisplayState.Unlock || displayState == DisplayState.Wrong)
            return;

        // optional: hiển thị dạng **** thay vì số thật
        displayText.text = currentInput;
    }
}