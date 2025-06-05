using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightToggle : MonoBehaviour
{
    public Light flashlight; // dein SpotLight
    private XRIDefaultInputActions input;

    private void Awake()
    {
        input = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.XRILeftHandInteraction.Activate.performed += ToggleLight;
    }

    private void OnDisable()
    {
        input.XRILeftHandInteraction.Activate.performed -= ToggleLight;
        input.Disable();
    }

    private void ToggleLight(InputAction.CallbackContext ctx)
    {
        if (flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
