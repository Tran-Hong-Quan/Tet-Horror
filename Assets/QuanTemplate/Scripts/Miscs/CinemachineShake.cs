using System.Collections;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineCam;
    private CinemachineBasicMultiChannelPerlin noise;

    private NoiseSettings defaultNoiseProfile;
    private float defaultAmplitude;
    private float defaultFrequency;

    [SerializeField] private NoiseSettings shakeNoiseProfile; // Profile 6D Shake

    private void Awake()
    {
        cinemachineCam = GetComponent<CinemachineVirtualCamera>();

        if (cinemachineCam != null)
        {
            noise = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise != null)
            {
                // Lưu giá trị Noise mặc định
                defaultNoiseProfile = noise.m_NoiseProfile;
                defaultAmplitude = noise.m_AmplitudeGain;
                defaultFrequency = noise.m_FrequencyGain;
            }
        }
    }

    /// <summary>
    /// Rung camera bằng 6D shake, sau đó reset về Noise Profile cũ
    /// </summary>
    /// <param name="intensity">Cường độ rung</param>
    /// <param name="frequency">Tần số rung</param>
    /// <param name="duration">Thời gian rung</param>
    public void ShakeCamera(float intensity, float frequency, float duration)
    {
        if (noise == null || shakeNoiseProfile == null) return;

        // Đổi sang Noise Profile 6D shake
        noise.m_NoiseProfile = shakeNoiseProfile;
        noise.m_AmplitudeGain = intensity;
        noise.m_FrequencyGain = frequency;

        // Reset sau khi rung xong
        StartCoroutine(ResetShake(duration));
    }

    private IEnumerator ResetShake(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (noise != null)
        {
            // Khôi phục lại Noise Profile cũ và giá trị ban đầu
            noise.m_NoiseProfile = defaultNoiseProfile;
            noise.m_AmplitudeGain = defaultAmplitude;
            noise.m_FrequencyGain = defaultFrequency;
        }
    }
}
