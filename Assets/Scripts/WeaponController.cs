using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform firePoint;
    public float range = 20f;
    public LineRenderer laserBeam;
    public AudioSource fireSound;

    private XRIDefaultInputActions inputActions;

    private void Awake()
    {
        inputActions = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.XRIRightHand.Fire.performed += FireWeapon;
    }

    private void OnDisable()
    {
        inputActions.XRIRightHand.Fire.performed -= FireWeapon;
        inputActions.Disable();
    }

    private void FireWeapon(InputAction.CallbackContext context)
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Ghost"))
            {
                Destroy(hit.collider.gameObject);
            }

            // Zeige Laserstrahl
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
            yield return new WaitForSeconds(0.1f);
            laserBeam.enabled = false;
        }
    }
}
