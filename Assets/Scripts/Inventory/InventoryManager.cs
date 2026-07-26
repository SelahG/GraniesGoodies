using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private WagonSpawner wagonSpawner;

    private readonly Dictionary<PlantType, int> inventory =
        new Dictionary<PlantType, int>();

    private readonly Dictionary<PlantType, int> required =
        new Dictionary<PlantType, int>();

    public bool IsComplete { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate InventoryManager destroyed.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        RegisterScenePlants();
    }

    private void RegisterScenePlants()
    {
        PlantID[] plants = FindObjectsOfType<PlantID>(true);

        foreach (PlantID plant in plants)
        {
            RegisterPlant(plant);
        }
    }

    public void RegisterPlant(PlantID plant)
    {
        if (plant == null)
        {
            Debug.LogWarning("Cannot register a missing PlantID.");
            return;
        }

        if (required.ContainsKey(plant.plantType))
        {
            return;
        }

        int amountNeeded = Mathf.Max(1, plant.amountNeeded);

        required.Add(plant.plantType, amountNeeded);
        inventory.Add(plant.plantType, 0);

        IsComplete = false;
    }

    public bool CollectPlant(PlantID plant)
    {
        if (plant == null)
        {
            Debug.LogWarning("Cannot collect a plant without a PlantID.");
            return false;
        }

        RegisterPlant(plant);

        PlantType plantType = plant.plantType;

        if (inventory[plantType] >= required[plantType])
        {
            Debug.Log($"{plantType} requirement is already complete.");
            return false;
        }

        if (wagonSpawner == null)
        {
            Debug.LogError("InventoryManager has no WagonSpawner assigned.");
            return false;
        }

        if (plant.wagonPrefab == null)
        {
            Debug.LogError($"{plant.name} has no wagon prefab assigned.");
            return false;
        }

        bool plantSpawned = wagonSpawner.SpawnPlant(plant.wagonPrefab);

        if (!plantSpawned)
        {
            return false;
        }

        inventory[plantType]++;

        Debug.Log(
            $"Collected {plantType}: " +
            $"{inventory[plantType]}/{required[plantType]}"
        );

        CheckForCompletion();

        return true;
    }

    private void CheckForCompletion()
    {
        if (required.Count == 0)
        {
            return;
        }

        foreach (KeyValuePair<PlantType, int> plant in required)
        {
            if (inventory[plant.Key] < plant.Value)
            {
                IsComplete = false;
                return;
            }
        }

        if (IsComplete)
        {
            return;
        }

        IsComplete = true;

        Debug.Log("Everything collected!");

        // TODO: Trigger Granny cutscene.
    }

    public int GetCollectedAmount(PlantType plantType)
    {
        if (inventory.TryGetValue(plantType, out int amount))
        {
            return amount;
        }

        return 0;
    }

    public int GetRequiredAmount(PlantType plantType)
    {
        if (required.TryGetValue(plantType, out int amount))
        {
            return amount;
        }

        return 0;
    }
}