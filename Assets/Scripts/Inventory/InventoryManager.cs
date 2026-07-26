using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("References")]
    public WagonSpawner wagonSpawner;

    private Dictionary<PlantType, int> inventory = new();
    private Dictionary<PlantType, int> required = new();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterPlant(PlantID plant)
    {
        if (!required.ContainsKey(plant.plantType))
        {
            required.Add(plant.plantType, plant.amountNeeded);
            inventory.Add(plant.plantType, 0);
        }
    }

    public bool CollectPlant(PlantID plant)
    {
        RegisterPlant(plant);

        if (inventory[plant.plantType] >= required[plant.plantType])
            return false;

        inventory[plant.plantType]++;

        wagonSpawner.SpawnPlant(plant.wagonPrefab);

        CheckForCompletion();

        return true;
    }

    void CheckForCompletion()
    {
        foreach (var plant in required)
        {
            if (inventory[plant.Key] < plant.Value)
                return;
        }

        Debug.Log("Everything collected!");
        // TODO Trigger Granny cutscene.
    }
}
