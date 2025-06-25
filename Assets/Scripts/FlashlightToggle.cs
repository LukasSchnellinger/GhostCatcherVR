using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class FlashlightToggle : MonoBehaviour
{
    [Header("Setup")]
    public Light flashlight;                            // Dein Light-Kegel
    private InputAction activateAction;

    [Header("Audio Clips")]
    public AudioClip toggleOnClip;                      // Sound beim Anknipsen
    public AudioClip toggleOffClip;                     // Sound beim Ausschalten
    public AudioClip humLoopClip;                       // Loop-Sound, wenn Lampe an ist

    private AudioSource audioSource;
    private bool isHumPlaying = false;
    private XRIDefaultInputActions inputActions;

    private void Awake()
    {
        // Input-Actions initialisieren
        inputActions = new XRIDefaultInputActions();
        activateAction = inputActions.XRILeftHandInteraction.Activate;

        // AudioSource vorbereiten
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false; // steuern wir manuell
    }

    private void OnEnable()
    {
        activateAction.Enable();
        activateAction.performed += OnToggle;
    }

    private void OnDisable()
    {
        activateAction.performed -= OnToggle;
        activateAction.Disable();
    }

    private void OnToggle(InputAction.CallbackContext ctx)
    {
        if (flashlight == null) return;

        // Lampe umschalten
        flashlight.enabled = !flashlight.enabled;
        bool nowOn = flashlight.enabled;

        // Sofort Ein-/Aus-Sound abspielen
        audioSource.loop = false;
        audioSource.clip = nowOn ? toggleOnClip : toggleOffClip;
        audioSource.Play();

        // Hum-Loop starten oder stoppen
        if (nowOn)
        {
            // nach dem Toggle-Clip den Loop starten
            StartCoroutine(StartHumAfterDelay(audioSource.clip.length));
        }
        else
        {
            // sofort stoppen
            if (isHumPlaying)
            {
                audioSource.Stop();
                isHumPlaying = false;
            }
        }
    }

    private System.Collections.IEnumerator StartHumAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.clip = humLoopClip;
        audioSource.loop = true;
        audioSource.Play();
        isHumPlaying = true;
    }
}
