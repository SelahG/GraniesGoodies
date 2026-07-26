using UnityEngine;

public enum PlantType
{
    Lavender,
    Bluebell,
    Rose,
    Mushroom,
    Nightshade,
    Moss,
    Daisy,
    Fern
}

[DisallowMultipleComponent]
public class PlantID : MonoBehaviour
{
    [Header("Plant Information")]

    [Tooltip("The type of plant being collected.")]
    public PlantType plantType;

    [Min(1)]
    [Tooltip("How many plants of this type Granny needs.")]
    public int amountNeeded = 5;

    [Tooltip("Prefab placed in the wagon when this plant is collected.")]
    public GameObject wagonPrefab;

    private void OnValidate()
    {
        amountNeeded = Mathf.Max(1, amountNeeded);
    }
}