using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnInterval = 3f;
    public float spawnRange = 3f;
    public int maxGhosts = 10;

    private float timer = 0f;
    private int activeGhosts = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && activeGhosts < maxGhosts)
        {
            SpawnGhost();
            timer = 0f;
        }
    }

    void SpawnGhost()
    {
        Vector3 spawnPos = transform.position + new Vector3(
            Random.Range(-spawnRange, spawnRange),
            Random.Range(1f, 2f),  // leicht über Bodenhöhe
            Random.Range(-spawnRange, spawnRange)
        );

        GameObject ghost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
        activeGhosts++;

        // Automatisch zählen, wenn Geist zerstört wird
        ghost.AddComponent<GhostDespawnTracker>().spawner = this;
    }

    public void NotifyGhostDestroyed()
    {
        activeGhosts--;
    }
}
