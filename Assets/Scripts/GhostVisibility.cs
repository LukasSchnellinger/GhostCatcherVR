using UnityEngine;

public class GhostVisibility : MonoBehaviour
{
    private Renderer ghostRenderer;
    private float currentAlpha = 0f;
    public float visibleAlpha = 0.8f;
    public float fadeSpeed = 3f;

    void Start()
    {
        ghostRenderer = GetComponent<Renderer>();
        SetAlpha(0f); // unsichtbar
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("LightCone"))
        {
            SetAlpha(Mathf.Lerp(currentAlpha, visibleAlpha, Time.deltaTime * fadeSpeed));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LightCone"))
        {
            SetAlpha(0f);
        }
    }

    void SetAlpha(float value)
    {
        currentAlpha = value;
        Color c = ghostRenderer.material.color;
        c.a = currentAlpha;
        ghostRenderer.material.color = c;
    }
}
