using UnityEngine;

[RequireComponent(typeof(PlantID))]
public class PlantCollectable : MonoBehaviour
{
    private PlantID plantID;
    private bool collected;

    private void Awake()
    {
        plantID = GetComponent<PlantID>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player"))
        {
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("No InventoryManager exists in the scene.");
            return;
        }

        bool accepted =
            InventoryManager.Instance.CollectPlant(plantID);

        if (!accepted)
        {
            return;
        }

        collected = true;
        Destroy(gameObject);
    }
}