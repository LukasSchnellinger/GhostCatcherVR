using UnityEngine;

public class LampFlicker : MonoBehaviour
{
    public Light lampLight;
    public float flickerSpeed = 20f;
    public float intensityMin = 3f;
    public float intensityMax = 5f;

    void Update()
    {
        if (lampLight != null && lampLight.enabled)
        {
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);
            lampLight.intensity = Mathf.Lerp(intensityMin, intensityMax, noise);
        }
    }
}
