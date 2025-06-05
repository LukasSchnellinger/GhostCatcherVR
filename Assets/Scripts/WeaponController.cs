using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    public float range = 20f;
    public LineRenderer laserBeam;
    public AudioSource fireSound;
    public Material laserMaterial;


    private XRIDefaultInputActions input;

    private void Awake()
    {
        input = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.XRIRightHandInteraction.Activate.performed += FireWeapon;
    }

    private void OnDisable()
    {
        input.XRIRightHandInteraction.Activate.performed -= FireWeapon;
        input.Disable();
    }

    private void FireWeapon(InputAction.CallbackContext ctx)
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Ghost"))
            {
                Destroy(hit.collider.gameObject);
            }
            StartCoroutine(FireEffect(hit.point));
        }
        else
        {
            StartCoroutine(FireEffect(ray.origin + ray.direction * range));
        }

        if (fireSound) fireSound.Play();
    }

    private System.Collections.IEnumerator FireEffect(Vector3 hitPoint)
    {
        if (laserBeam != null)
        {
            laserBeam.SetPosition(0, firePoint.position);
            laserBeam.SetPosition(1, hitPoint);
            laserBeam.enabled = true;

            float duration = 0.2f;
            float elapsed = 0f;
            Color baseColor = Color.red;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float pulse = Mathf.PingPong(Time.time * 5f, 1f);
                Color animated = baseColor * Mathf.Lerp(1f, 3f, pulse);

                if (laserMaterial != null)
                {
                    laserMaterial.SetColor("_BaseColor", animated); // Shader: URP/Unlit
                    laserMaterial.SetColor("_EmissionColor", animated); // für Legacy/Standard Shader
                }

                yield return null;
            }

            laserBeam.enabled = false;
        }
    }
}
