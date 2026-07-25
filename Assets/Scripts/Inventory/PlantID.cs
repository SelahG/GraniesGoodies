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

public class PlantID : MonoBehaviour
{
    [Header("Plant Info")]
    public PlantType plantType;

    [Tooltip("How many Granny needs.")]
    public int amountNeeded = 5;

    [Tooltip("Prefab to place in the wagon.")]
    public GameObject wagonPrefab;
}