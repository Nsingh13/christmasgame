using UnityEngine;

public class IcicleSpawner : MonoBehaviour
{
    public GameObject iciclePrefab; // Prefab for the icicle
    public float spawnIntervalMin = 2f; // Minimum time between spawns
    public float spawnIntervalMax = 5f; // Maximum time between spawns
    public float spawnDistanceAhead = 10f; // Distance ahead of the camera to spawn
    public float spawnHeight = 5f; // Fixed height for icicle spawn

    private Camera mainCamera;
    private float nextSpawnTime;

    void Start()
    {
        mainCamera = Camera.main;
        ScheduleNextSpawn();
    }

    void Update()
    {
        // Check if it's time to spawn the next icicle
        if (Time.time >= nextSpawnTime)
        {
            SpawnIcicle();
            ScheduleNextSpawn();
        }
    }

    private void SpawnIcicle()
    {
        // Get the camera's position and calculate the spawn position
        Vector3 cameraPosition = mainCamera.transform.position;
        float spawnX = cameraPosition.x + spawnDistanceAhead;

        // Create the icicle at the spawn position
        Instantiate(iciclePrefab, new Vector3(spawnX, spawnHeight, 0), Quaternion.identity);
    }

    private void ScheduleNextSpawn()
    {
        // Schedule the next spawn at a random interval
        nextSpawnTime = Time.time + Random.Range(spawnIntervalMin, spawnIntervalMax);
    }
}
