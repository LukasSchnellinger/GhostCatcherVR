using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRayDetector : MonoBehaviour
{
    public float detectionRange = 15f;       
    public LayerMask ghostLayer;              

    private GhostBehavior lastHitGhost = null;

    void Update()
    {
        RaycastHit hit;
        
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, ghostLayer))
        {
            GhostBehavior ghost = hit.collider.GetComponent<GhostBehavior>();
            
            if (ghost != null)
            {
                
                if (ghost != lastHitGhost)
                {
                    if (lastHitGhost != null)
                        lastHitGhost.SetInLight(false); 

                    ghost.SetInLight(true);
                    lastHitGhost = ghost;
                }
            }
        }
        else
        {
            
            if (lastHitGhost != null)
            {
                lastHitGhost.SetInLight(false);
                lastHitGhost = null;
            }
        }
    }
}