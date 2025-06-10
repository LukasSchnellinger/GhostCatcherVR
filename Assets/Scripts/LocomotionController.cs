using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class LocomotionController : MonoBehaviour
{
    public Transform headTransform;       // Main Camera vom XR Origin
    public float moveSpeed = 3f;

    private CharacterController cc;
    private XRIDefaultInputActions input;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        input = new XRIDefaultInputActions();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        // 1) Lese Stick-Eingabe
        Vector2 stick = input.XRILeftHandLocomotion.Move.ReadValue<Vector2>();

        // 2) Baue Bewegungsvektor in Weltkoordinaten
        Vector3 forward = Vector3.ProjectOnPlane(headTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(headTransform.right, Vector3.up).normalized;
        Vector3 motion = (forward * stick.y + right * stick.x) * moveSpeed * Time.deltaTime;

        // 3) Bewegung mit Kollision
        cc.Move(motion);
    }
}
