using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    public float range = 20f;
    public LineRenderer laserBeam;
    public AudioSource fireSound;
    public float fireCooldown = 1.5f;
    private float nextFireTime = 0f;

    public GameObject shootVFXPrefab;

    // 🔹 Neu: Ghost-Layer einstellen
    public LayerMask ghostLayer;

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
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireCooldown;

        // 1) VFX beim Schuss
        if (shootVFXPrefab != null)
        {
            GameObject vfx = Instantiate(shootVFXPrefab, firePoint.position, firePoint.rotation);
            Destroy(vfx, 2f);
        }

        // 2) Raycast mit GhostLayer
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range, ghostLayer))
        {
            // 🔹 Geist erkennen
            GhostBehavior ghost = hit.collider.GetComponent<GhostBehavior>();
            if (ghost != null)
            {
                ghost.TakeDamage(1); // 1 Schaden pro Treffer
            }

            StartCoroutine(FireEffect(hit.point));
        }
        else
        {
            StartCoroutine(FireEffect(ray.origin + ray.direction * range));
        }

        // 3) Sound abspielen
        if (fireSound != null) fireSound.Play();
    }

    private System.Collections.IEnumerator FireEffect(Vector3 hitPoint)
    {
        if (laserBeam != null)
        {
            laserBeam.SetPosition(0, firePoint.position);
            laserBeam.SetPosition(1, hitPoint);
            laserBeam.enabled = true;
            yield return new WaitForSeconds(0.1f);
            laserBeam.enabled = false;
        }
    }
}