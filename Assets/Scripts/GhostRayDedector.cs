using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRayDetector : MonoBehaviour
{
    public float detectionRange = 15f;
    public LayerMask ghostLayer;

    private List<GhostBehavior> litGhosts = new List<GhostBehavior>();

    void Update()
    {
       
        foreach (GhostBehavior ghost in litGhosts)
        {
            if (ghost != null)
                ghost.SetInLight(false);
        }
        litGhosts.Clear();

        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, detectionRange, ghostLayer);

        foreach (RaycastHit hit in hits)
        {
            GhostBehavior ghost = hit.collider.GetComponent<GhostBehavior>();
            if (ghost != null)
            {
                ghost.SetInLight(true);
                litGhosts.Add(ghost);
            }
        }
    }
}