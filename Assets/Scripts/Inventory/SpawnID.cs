using UnityEngine;

[DisallowMultipleComponent]
public class SpawnID : MonoBehaviour
{
    [Tooltip("Lower numbers fill first.")]
    [Min(0)]
    public int spawnOrder;

    [HideInInspector]
    public bool occupied;
}