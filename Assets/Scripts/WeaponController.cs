using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    public float range = 20f;
    public LineRenderer laserBeam;
    public AudioSource fireSound;
    public float fireCooldown = 1.5f; // Cooldown-Zeit in Sekunden
    public float nextFireTime = 0f;

    // NEU: VFX-Prefab-Referenz
    public GameObject shootVFXPrefab;

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
        // Cooldown-Check
        if (Time.time < nextFireTime) return; // Abbrechen, wenn Cooldown aktiv ist
        nextFireTime = Time.time + fireCooldown; // Nächster Schusszeitpunkt setzen

        // 1) Instanziere den VFX-Effekt an firePoint
        if (shootVFXPrefab != null)
        {
            GameObject vfx = Instantiate(shootVFXPrefab, firePoint.position, firePoint.rotation);
            // falls das Prefab automatisch zerstört wird, ok. Sonst:
            Destroy(vfx, 2f); // löscht den Effekt nach 2 Sekunden
        }

        // 2) Laser-Beam wie gehabt
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Ghost"))
                Destroy(hit.collider.gameObject);

            StartCoroutine(FireEffect(hit.point));
        }
        else
        {
            StartCoroutine(FireEffect(ray.origin + ray.direction * range));
        }

        // 3) Sound
        if (fireSound) fireSound.Play();
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
