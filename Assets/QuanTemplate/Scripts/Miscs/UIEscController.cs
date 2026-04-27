using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEscController : MonoBehaviour
{
    private static UIEscController instance;
    public static UIEscController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("UIEscController");
                instance = obj.AddComponent<UIEscController>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private Stack<Button> buttonStack = new Stack<Button>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscape();
        }
    }

    void HandleEscape()
    {
        while (buttonStack.Count > 0)
        {
            Button top = buttonStack.Peek();

            if (top == null)
            {
                buttonStack.Pop();
                continue;
            }

            if (top.gameObject.activeInHierarchy && top.interactable)
            {
                top.onClick.Invoke();
                return;
            }
            else
            {
                buttonStack.Pop();
            }
        }

        Debug.Log("No UI button to handle Esc");
    }

    // ===== REGISTER / UNREGISTER =====
    public void Register(Button btn)
    {
        if (btn == null) return;
        if (buttonStack.Contains(btn)) return;

        buttonStack.Push(btn);
    }

    public void Unregister(Button btn)
    {
        if (btn == null) return;

        Stack<Button> temp = new Stack<Button>();

        while (buttonStack.Count > 0)
        {
            var top = buttonStack.Pop();
            if (top != btn)
                temp.Push(top);
        }

        while (temp.Count > 0)
            buttonStack.Push(temp.Pop());
    }
}