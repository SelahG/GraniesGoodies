using UnityEngine;

[RequireComponent(typeof(PlantID))]
public class PlantCollectable : MonoBehaviour
{
    private PlantID plantID;
    private bool hasBeenCollected;

    private void Awake()
    {
        plantID = GetComponent<PlantID>();
    }

    public void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("No InventoryManager exists in the scene.");
            return;
        }

        bool accepted = InventoryManager.Instance.CollectPlant(plantID);

        if (!accepted)
        {
            return;
        }

        hasBeenCollected = true;
        Destroy(gameObject);
    }
}