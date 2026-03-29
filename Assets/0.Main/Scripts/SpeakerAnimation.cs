using UnityEngine;

public class SpeakerAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float baseAmplitude = 0.2f; // Biên độ rung
    [SerializeField] private float baseFrequency = 10f; // Tần số
    [SerializeField] private float speedMultiplier = 1f; // Tốc độ tổng
    [SerializeField] private bool useAudioInput = false; // Sử dụng input âm thanh thực tế

    [Header("Audio Input (Optional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float audioSensitivity = 10f;
    [SerializeField] private int sampleDataLength = 1024;

    [Header("Multi-band Animation")]
    [SerializeField] private bool useMultiBand = true;
    [SerializeField] private float[] bandMultipliers = { 1f, 1.5f, 2f, 1f }; // Nhân cho các dải tần

    private float[] clipSampleData;
    private float currentAmplitude = 0f;
    private Vector3 originalScale;
    private float timer = 0f;

    void Start()
    {
        originalScale = transform.localScale;

        if (useAudioInput && audioSource != null)
        {
            clipSampleData = new float[sampleDataLength];
        }
    }

    void Update()
    {
        if (useAudioInput && audioSource != null)
        {
            GetAudioAmplitude();
        }
        else
        {
            // Dùng sin wave mô phỏng nếu không có audio input
            currentAmplitude = Mathf.Sin(Time.time * baseFrequency * speedMultiplier) * baseAmplitude;
        }

        AnimateSpeaker();
    }

    void GetAudioAmplitude()
    {
        audioSource.GetOutputData(clipSampleData, 0);

        float sum = 0f;
        for (int i = 0; i < sampleDataLength; i++)
        {
            sum += Mathf.Abs(clipSampleData[i]);
        }

        currentAmplitude = (sum / sampleDataLength) * audioSensitivity;
    }

    void AnimateSpeaker()
    {
        timer += Time.deltaTime * speedMultiplier;

        if (useMultiBand)
        {
            AnimateMultiBand();
        }
        else
        {
            AnimateSimple();
        }
    }

    void AnimateSimple()
    {
        // Rung đơn giản
        float pulse = Mathf.Sin(timer * baseFrequency) * currentAmplitude * baseAmplitude;
        Vector3 newScale = originalScale + Vector3.one * pulse;
        transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * 10f);
    }

    void AnimateMultiBand()
    {
        // Rung đa tần số tạo hiệu ứng phức tạp hơn
        float animationValue = 0f;

        for (int i = 0; i < bandMultipliers.Length; i++)
        {
            float frequency = baseFrequency * (i + 1);
            float amplitude = currentAmplitude * baseAmplitude * bandMultipliers[i];
            animationValue += Mathf.Sin(timer * frequency) * amplitude;
        }

        // Chuẩn hóa giá trị
        animationValue /= bandMultipliers.Length;

        // Áp dụng animation cho scale
        Vector3 newScale = originalScale * (1 + animationValue);
        transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * 15f);
    }

    // Phương thức public để điều khiển từ bên ngoài
    public void SetIntensity(float intensity)
    {
        baseAmplitude = Mathf.Clamp(intensity, 0.1f, 1f);
    }

    public void SetSpeed(float speed)
    {
        speedMultiplier = Mathf.Clamp(speed, 0.5f, 3f);
    }

    public void SetAudioSource(AudioSource source)
    {
        audioSource = source;
        if (audioSource != null)
        {
            useAudioInput = true;
            clipSampleData = new float[sampleDataLength];
        }
    }
}