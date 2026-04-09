using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [Header("Spawner References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Camera spawnCamera;

    [Header("Wave Growth")]
    [SerializeField] private int startingEnemiesPerWave = 2;
    [SerializeField] private int startingTierWaveCount = 2;
    [SerializeField] private int enemiesPerTierIncrease = 2;
    [SerializeField] private int wavesPerTierIncrease = 1;

    [Header("Spawn Placement")]
    [SerializeField] private float edgeInset = 0f;
    [SerializeField] private float fallbackSpawnRadius = 8f;

    private readonly List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWaveNumber;

    private void Start()
    {
        SpawnNextWave();
    }

    private void Update()
    {
        CleanupClearedEnemies();

        if (activeEnemies.Count == 0)
        {
            SpawnNextWave();
        }
    }

    private void CleanupClearedEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
            }
        }
    }

    private void SpawnNextWave()
    {
        if (enemyPrefab == null)
        {
            enabled = false;
            return;
        }

        currentWaveNumber++;
        int enemyCount = GetEnemyCountForWave(currentWaveNumber);

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefab, GetEdgeSpawnPosition(), Quaternion.identity);
            activeEnemies.Add(spawnedEnemy);
        }
    }

    private int GetEnemyCountForWave(int waveNumber)
    {
        int wavesInTier = Mathf.Max(1, startingTierWaveCount);
        int enemiesInTier = Mathf.Max(1, startingEnemiesPerWave);
        int tierStartWave = 1;

        while (waveNumber >= tierStartWave + wavesInTier)
        {
            tierStartWave += wavesInTier;
            wavesInTier = Mathf.Max(1, wavesInTier + wavesPerTierIncrease);
            enemiesInTier = Mathf.Max(1, enemiesInTier + enemiesPerTierIncrease);
        }

        return enemiesInTier;
    }

    private Vector3 GetEdgeSpawnPosition()
    {
        Camera activeCamera = spawnCamera != null ? spawnCamera : Camera.main;
        if (activeCamera == null)
        {
            Vector2 fallbackPosition = Random.insideUnitCircle.normalized * fallbackSpawnRadius;
            return transform.position + new Vector3(fallbackPosition.x, fallbackPosition.y, 0f);
        }

        float planeDistance = Mathf.Abs(transform.position.z - activeCamera.transform.position.z);
        Vector3 bottomLeft = activeCamera.ViewportToWorldPoint(new Vector3(0f, 0f, planeDistance));
        Vector3 topRight = activeCamera.ViewportToWorldPoint(new Vector3(1f, 1f, planeDistance));

        float minX = bottomLeft.x + edgeInset;
        float maxX = topRight.x - edgeInset;
        float minY = bottomLeft.y + edgeInset;
        float maxY = topRight.y - edgeInset;
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0:
                return new Vector3(Random.Range(minX, maxX), maxY, transform.position.z);
            case 1:
                return new Vector3(Random.Range(minX, maxX), minY, transform.position.z);
            case 2:
                return new Vector3(minX, Random.Range(minY, maxY), transform.position.z);
            default:
                return new Vector3(maxX, Random.Range(minY, maxY), transform.position.z);
        }
    }
}
