using System;
using UnityEngine;

public class WagonSpawner : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Parent used to organize plants spawned in the wagon.")]
    [SerializeField] private Transform decorationParent;

    private SpawnID[] spawnPoints;

    private void Awake()
    {
        CacheSpawnPoints();
    }

    private void CacheSpawnPoints()
    {
        spawnPoints = GetComponentsInChildren<SpawnID>(true);

        Array.Sort(
            spawnPoints,
            (first, second) =>
                first.spawnOrder.CompareTo(second.spawnOrder)
        );

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning(
                $"WagonSpawner on {gameObject.name} has no SpawnID children."
            );
        }

        if (decorationParent == null)
        {
            decorationParent = transform;

            Debug.LogWarning(
                $"No decoration parent assigned to {gameObject.name}. " +
                "Spawned plants will use the WagonSpawner as their parent."
            );
        }
    }

    public bool SpawnPlant(GameObject wagonPrefab)
    {
        if (wagonPrefab == null)
        {
            Debug.LogError("Cannot spawn a null wagon plant prefab.");
            return false;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No wagon spawn points are available.");
            return false;
        }

        foreach (SpawnID spawnPoint in spawnPoints)
        {
            if (spawnPoint == null || spawnPoint.occupied)
            {
                continue;
            }

            GameObject spawnedPlant = Instantiate(
                wagonPrefab,
                spawnPoint.transform.position,
                spawnPoint.transform.rotation,
                decorationParent
            );

            spawnedPlant.transform.localScale =
                wagonPrefab.transform.localScale;

            spawnPoint.occupied = true;

            return true;
        }

        Debug.LogWarning("No free wagon spawn points remain.");
        return false;
    }

    public int GetRemainingSpace()
    {
        int remainingSpace = 0;

        foreach (SpawnID spawnPoint in spawnPoints)
        {
            if (spawnPoint != null && !spawnPoint.occupied)
            {
                remainingSpace++;
            }
        }

        return remainingSpace;
    }
}