using UnityEngine;

public class SpawnID : MonoBehaviour
{
    [Tooltip("Lower numbers fill first.")]
    public int spawnOrder;

    [HideInInspector]
    public bool occupied = false;
}
