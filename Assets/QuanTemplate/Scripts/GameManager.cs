using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;
using UnityEngine.SceneManagement;
using CodeStage.AdvancedFPSCounter;

public class GameManager 
{
    public const string LEVEL_1_PREFIX = "Level_1_";

    [Command("camera-sensitivity")]
    public static float cameraSensitivity = 2f;

    [Command("scene-reload")]
    public static void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    [Command("quit")]
    public static void QuitGame()
    {
        Application.Quit();
    }

        [Command("set-cursor-lock-state")]
    public static void SetCursorLockState(string state)
    {
        switch (state.ToLower())
        {
            case "none":
                Cursor.lockState = CursorLockMode.None;
                break;
            case "locked":
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case "confined":
                Cursor.lockState = CursorLockMode.Confined;
                break;
            default:
                Debug.LogError("Invalid cursor lock state. Use 'none', 'locked', or 'confined'.");
                break;
        }
    }

    public static void SaveCameraSensitivity()
    {
        PlayerPrefs.SetFloat("Camera sensitivity", cameraSensitivity);
    }

    public static void LoadCameraSensitivity()
    {
#if UNITY_ANDROID || UNITY_IOS
        cameraSensitivity = PlayerPrefs.GetFloat("Camera sensitivity", 2);
#else
        cameraSensitivity = PlayerPrefs.GetFloat("Camera sensitivity", 1);
#endif
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnGameStart()
    {
        GameObject.Instantiate(Resources.Load<GameObject>("Advanced FPS Counter"));
    }
}