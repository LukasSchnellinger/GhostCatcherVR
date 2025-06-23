using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;

    private bool isInLight = false;
    private Renderer ghostRenderer;

    void Start()
    {
        ghostRenderer = GetComponent<Renderer>();

        if (ghostRenderer != null)
        {
            ghostRenderer.enabled = false; 
        }
    }

    void Update()
    {
        
        if (isInLight && player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    
    public void SetInLight(bool value)
    {
        isInLight = value;

        if (ghostRenderer != null)
        {
            ghostRenderer.enabled = value; 
        }
    }
}