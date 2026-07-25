using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChecklistUI : MonoBehaviour
{
    public TMP_Text checklist;

    public void UpdateChecklist(
        Dictionary<PlantType, int> inventory,
        Dictionary<PlantType, int> required)
    {
        checklist.text = "";

        foreach (var plant in required)
        {
            bool complete = inventory[plant.Key] >= plant.Value;

            checklist.text += complete
                ? $"<s>✓ {plant.Key} ({inventory[plant.Key]}/{plant.Value})</s>\n"
                : $"☐ {plant.Key} ({inventory[plant.Key]}/{plant.Value})\n";
        }
    }
}
