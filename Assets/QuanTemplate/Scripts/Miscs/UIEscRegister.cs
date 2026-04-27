using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIEscRegister : MonoBehaviour
{
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
    }

    void OnEnable()
    {
        UIEscController.Instance.Register(btn);
    }

    private void OnDisable()
    {
        UIEscController.Instance.Unregister(btn);
    }
}