using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCameraShake : MonoBehaviour
{
    public CinemachineShake cameraShake;
    public float intensity = 2.5f;
    public float frequency = 1f; 
    public float duration = .5f;

    public void Play()
    {
        cameraShake.ShakeCamera(intensity, frequency, duration);
    }
}
