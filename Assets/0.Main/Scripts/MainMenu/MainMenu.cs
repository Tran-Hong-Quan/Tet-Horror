using CodeStage.AdvancedFPSCounter;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image transitionImage;
    public float transitionDuration = 1f;
    public GameObject thankBoard;

    public string startGameSceneName = "Room 1";

    const string OPEN_THANK_BOARD_ON_START_KEY = "OpenThankBoardOnStart";
    const string OPEN_CHANGE_LANGUAGE_BOARD_ON_START_KEY = "OpenChangeLanguageBoardOnStart";

    private void Start()
    {
        if (PlayerPrefs.GetInt(OPEN_THANK_BOARD_ON_START_KEY, 0) == 1)
        {
            PlayerPrefs.SetInt(OPEN_THANK_BOARD_ON_START_KEY, 0);
            thankBoard.SetActive(true);
        }
        Application.targetFrameRate = 120;
    }

    public static void SetOpenThankBoardOnStart()
    {
        PlayerPrefs.SetInt(OPEN_THANK_BOARD_ON_START_KEY, 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    bool isStartGame = false;

    public void Play()
    {
        if (isStartGame) return;
        isStartGame = true;

        transitionImage.gameObject.SetActive(true);
        transitionImage.color = new Color(0, 0, 0, 0);
        transitionImage.DOFade(1, transitionDuration).OnComplete(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(startGameSceneName);
        });
    }

    public void OpenLink(string uri)
    {
        Application.OpenURL(uri);
    }

    public void ToggleFPS()
    {
        if (AFPSCounter.Instance.OperationMode == OperationMode.Normal)
        {
            AFPSCounter.Instance.OperationMode = OperationMode.Disabled;
        }
        else
        {
            AFPSCounter.Instance.OperationMode = OperationMode.Normal;
        }
    }
}
