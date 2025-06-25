using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("Setup")]
    public Transform firePoint;
    public LayerMask ghostLayer;

    [Header("FX")]
    public LineRenderer laserBeam;
    public GameObject shootVFXPrefab;
    public AudioClip fireSound;

    [Header("Schuss-Einstellungen")]
    public float range = 20f;
    public float fireCooldown = 1.5f;

    private float nextFireTime = 0f;
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

        // 🔹 VFX
        if (shootVFXPrefab && firePoint)
        {
            GameObject vfx = Instantiate(shootVFXPrefab, firePoint.position, firePoint.rotation);
            Destroy(vfx, 2f);
        }

        // 🔹 Raycast (mit Ghost-Layer)
        if (firePoint == null)
        {
            Debug.LogWarning("⚠️ Kein FirePoint gesetzt!");
            return;
        }

        Ray ray = new Ray(firePoint.position, firePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range, ghostLayer))
        {
            GhostBehavior ghost = hit.collider.GetComponent<GhostBehavior>();
            if (ghost != null)
            {
                ghost.TakeDamage(1);
                GameManager.Instance.AddKill();
                Debug.Log("💥 Geist getroffen!");
            }

            StartCoroutine(FireEffect(hit.point));
        }
        else
        {
            StartCoroutine(FireEffect(ray.origin + ray.direction * range));
        }

        // 🔹 Sound
        //if (fireSound) fireSound.Play();
    }

    private System.Collections.IEnumerator FireEffect(Vector3 hitPoint)
    {
        if (laserBeam != null)
        {
            //fireSound.Play();
            laserBeam.SetPosition(0, firePoint.position);
            laserBeam.SetPosition(1, hitPoint);
            laserBeam.enabled = true;
            yield return new WaitForSeconds(0.1f);
            laserBeam.enabled = false;
        }
    }
}