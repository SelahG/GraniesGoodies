using System.Linq;
using UnityEngine;

public class WagonSpawner : MonoBehaviour
{
    [Header("Parent for decorative plants")]
    public Transform decorationParent;

    private SpawnID[] spawnPoints;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<SpawnID>()
            .OrderBy(s => s.spawnOrder)
            .ToArray();
    }

    public bool SpawnPlant(GameObject wagonPrefab)
    {
        foreach (SpawnID spawn in spawnPoints)
        {
            if (spawn.occupied)
                continue;

            GameObject plant = Instantiate(
                wagonPrefab,
                spawn.transform.position,
                spawn.transform.rotation,
                decorationParent);

            plant.transform.localScale = wagonPrefab.transform.localScale;

            spawn.occupied = true;

            return true;
        }

        Debug.LogWarning("No free wagon spawn points!");
        return false;
    }
}