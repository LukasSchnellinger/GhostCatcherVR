using UnityEngine;

public class GhostDespawnTracker : MonoBehaviour
{
    public GhostSpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.NotifyGhostDestroyed();
        }
    }
}
