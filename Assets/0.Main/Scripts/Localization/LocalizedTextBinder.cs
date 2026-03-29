using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class LocalizedTextBinder : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private LocalizedString localizedString; 

    private void Awake()
    {
        if(text == null)
        {
            text = GetComponent<Text>();
        }
        localizedString.StringChanged += UpdateText;
    }

    private void OnEnable()
    {
        localizedString.RefreshString();
    }

    private void OnDisable()
    {
        localizedString.StringChanged -= UpdateText;
    }

    private void UpdateText(string value)
    {
        if (text == null)
        {
            return;
        }
        text.text = value;
    }
}
