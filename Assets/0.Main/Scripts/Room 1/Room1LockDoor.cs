using UnityEngine;
using UnityEngine.UI;

public class Room1LockDoor : InteractableDoor
{
    [Header("UI")]
    [SerializeField] GameObject lockPasswordCanvas;
    [SerializeField] Text displayText; // Kéo thả Text hiển thị
    [SerializeField] Button[] buttons; // Kéo thả tất cả nút 1-9,0,#,* vào đây

    [Header("Settings")]
    [SerializeField] string correctPassword = "1234"; // mật khẩu

    private string currentInput = "";
    private PlayerInteract playerInteract;

    private const string PlayerPrefKey = "Room1DoorUnlocked";

    bool isDoorUnlocked = false;

    protected override void Start()
    {
        base.Start();

        isDoorUnlocked = PlayerPrefs.GetInt(PlayerPrefKey, 0) == 1;

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
    public override void Interact(CharacterInteract characterInteract)
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

    private void OnButtonPressed(string key)
    {
        // Nếu đang hiển thị UNLOCK, không cần làm gì
        if (displayState == DisplayState.Unlock)
            return;

        if (key == "*")
        {
            // Xóa toàn bộ input
            currentInput = "";

            // Nếu đang hiển thị WRONG, reset về trạng thái Input
            if (displayState == DisplayState.Wrong)
                displayState = DisplayState.Input;
        }
        else if (key == "#")
        {
            // Kiểm tra mật khẩu
            if (currentInput == correctPassword)
            {
                displayState = DisplayState.Unlock;
                displayText.text = "UNLOCK";

                isDoorUnlocked = true;
                OpenOrCloseDoor();
                PlayerPrefs.SetInt(PlayerPrefKey, 1);
                PlayerPrefs.Save();
                return; // khỏi gọi UpdateDisplay
            }
            else
            {
                displayState = DisplayState.Wrong;
                displayText.text = "WRONG";
                currentInput = ""; // reset input để nhập lại
                return; // khỏi gọi UpdateDisplay
            }
        }
        else
        {
            // Bấm số, nếu trước đó hiển thị WRONG thì chuyển về Input
            if (displayState == DisplayState.Wrong)
                displayState = DisplayState.Input;

            currentInput += key;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        // Nếu đang hiển thị UNLOCK hoặc WRONG, không ghi đè nữa
        if (displayState == DisplayState.Unlock || displayState == DisplayState.Wrong)
            return;

        displayText.text = currentInput;
    }
}
