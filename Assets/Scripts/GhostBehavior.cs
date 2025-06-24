using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public int health = 1;

    private bool isInLight = false;
    private Renderer[] allRenderers;

    void Start()
    {
        // Alle Renderer-Komponenten sammeln (auch in Kindern)
        allRenderers = GetComponentsInChildren<Renderer>();

        SetVisibility(false); // Geist startet unsichtbar
    }

    void Update()
    {
        // Bewegung immer aktiv, unabhängig vom Licht
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    public void SetInLight(bool value)
    {
        isInLight = value;
        SetVisibility(value);
    }

    private void SetVisibility(bool visible)
    {
        foreach (Renderer r in allRenderers)
        {
            r.enabled = visible;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.Instance?.AddKill();
            Destroy(gameObject); // Geist entfernen, wenn keine Lebenspunkte
        }
    }
}