using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Sensitivity")]
    public Slider sensitivitySlider;
    public InputField sensitivityInput;
    private float sensitivity = 1f;
    private bool isUpdatingSensitivity = false;

    CursorLockMode lastLockMode;

    private void Start()
    {
        GameManager.LoadCameraSensitivity();
        sensitivity = GameManager.cameraSensitivity;
        sensitivitySlider.minValue = 0.1f;
        sensitivitySlider.maxValue = 10f;
        sensitivitySlider.value = sensitivity;
        sensitivityInput.text = sensitivity.ToString("F2");
        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);
        sensitivityInput.onEndEdit.AddListener(OnSensitivityInputChanged);
    }
    public void OutToMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Main Menu");
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        lastLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnCotinue()
    {
        Time.timeScale = 1;
        Cursor.lockState = lastLockMode;
    }
    void OnSensitivitySliderChanged(float value)
    {
        if (isUpdatingSensitivity) return;

        isUpdatingSensitivity = true;
        sensitivity = value;
        sensitivityInput.text = value.ToString("F2");
        isUpdatingSensitivity = false;

        // Gửi sensitivity tới camera nếu cần
        // CameraController.Instance.SetSensitivity(sensitivity);
        GameManager.cameraSensitivity = sensitivity;
    }

    void OnSensitivityInputChanged(string text)
    {
        if (isUpdatingSensitivity) return;

        if (float.TryParse(text, out float value))
        {
            value = Mathf.Clamp(value, sensitivitySlider.minValue, sensitivitySlider.maxValue);
            isUpdatingSensitivity = true;
            sensitivity = value;
            sensitivitySlider.value = value;
            sensitivityInput.text = value.ToString("F2");
            isUpdatingSensitivity = false;

            // CameraController.Instance.SetSensitivity(sensitivity);
            GameManager.cameraSensitivity = sensitivity;
        }
        else
        {
            // Reset lại nếu nhập sai
            sensitivityInput.text = sensitivity.ToString("F2");
        }
    }

    private void OnDestroy()
    {
        GameManager.SaveCameraSensitivity();
    }
}
