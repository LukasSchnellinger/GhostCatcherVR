using UnityEngine;

public class LampFlicker : MonoBehaviour
{
    public Light lampLight;
    public float flickerSpeed = 20f;
    public float intensityMin = 2f;
    public float intensityMax = 4f;

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);
        lampLight.intensity = Mathf.Lerp(intensityMin, intensityMax, noise);
    }
}
