using UnityEngine;

public class ProceduralIdleMotion : MonoBehaviour
{
    [Header("Enable")]
    public bool affectPosition = true;
    public bool affectRotation = true;

    [Header("Position Settings")]
    public Vector3 positionAmplitude = new Vector3(0.05f, 0.1f, 0.05f);
    public Vector3 positionFrequency = new Vector3(0.5f, 0.8f, 0.5f);

    [Header("Rotation Settings (degrees)")]
    public Vector3 rotationAmplitude = new Vector3(2f, 3f, 1f);
    public Vector3 rotationFrequency = new Vector3(0.5f, 0.7f, 0.4f);

    [Header("Noise (creepy randomness)")]
    public float noiseStrength = 0.02f;
    public float noiseSpeed = 0.5f;

    [Header("Head Twitch (optional horror effect)")]
    public bool enableTwitch = true;
    public float twitchChance = 0.02f; // per second
    public float twitchAmount = 8f;
    public float twitchDuration = 0.1f;

    private Vector3 startPos;
    private Quaternion startRot;

    private float twitchTimer;
    private Vector3 twitchOffset;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
    }

    void Update()
    {
        float t = Time.time;

        // -----------------------
        // POSITION (sine + noise)
        // -----------------------
        Vector3 posOffset = Vector3.zero;

        if (affectPosition)
        {
            posOffset.x = Mathf.Sin(t * positionFrequency.x) * positionAmplitude.x;
            posOffset.y = Mathf.Sin(t * positionFrequency.y) * positionAmplitude.y;
            posOffset.z = Mathf.Sin(t * positionFrequency.z) * positionAmplitude.z;

            // Perlin noise (creepy irregular motion)
            posOffset += new Vector3(
                (Mathf.PerlinNoise(t * noiseSpeed, 0f) - 0.5f) * noiseStrength,
                (Mathf.PerlinNoise(0f, t * noiseSpeed) - 0.5f) * noiseStrength,
                (Mathf.PerlinNoise(t * noiseSpeed, t * noiseSpeed) - 0.5f) * noiseStrength
            );
        }

        // -----------------------
        // ROTATION (sine + noise)
        // -----------------------
        Vector3 rotOffset = Vector3.zero;

        if (affectRotation)
        {
            rotOffset.x = Mathf.Sin(t * rotationFrequency.x) * rotationAmplitude.x;
            rotOffset.y = Mathf.Sin(t * rotationFrequency.y) * rotationAmplitude.y;
            rotOffset.z = Mathf.Sin(t * rotationFrequency.z) * rotationAmplitude.z;

            rotOffset += new Vector3(
                (Mathf.PerlinNoise(t * noiseSpeed + 10f, 0f) - 0.5f) * noiseStrength * 50f,
                (Mathf.PerlinNoise(0f, t * noiseSpeed + 10f) - 0.5f) * noiseStrength * 50f,
                (Mathf.PerlinNoise(t * noiseSpeed, t * noiseSpeed + 10f) - 0.5f) * noiseStrength * 50f
            );
        }

        // -----------------------
        // HORROR TWITCH SYSTEM
        // -----------------------
        if (enableTwitch)
        {
            if (twitchTimer <= 0f)
            {
                if (Random.value < twitchChance * Time.deltaTime)
                {
                    twitchTimer = twitchDuration;
                    twitchOffset = new Vector3(
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f)
                    ) * twitchAmount;
                }
            }
            else
            {
                twitchTimer -= Time.deltaTime;

                if (twitchTimer <= 0f)
                {
                    twitchOffset = Vector3.zero;
                }
            }

            rotOffset += twitchOffset;
        }

        // -----------------------
        // APPLY
        // -----------------------
        transform.localPosition = startPos + posOffset;
        transform.localRotation = startRot * Quaternion.Euler(rotOffset);
    }
}