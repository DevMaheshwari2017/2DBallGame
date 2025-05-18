using UnityEngine;

[CreateAssetMenu(fileName = "SpawnableObject", menuName = "Spawn System/SpawnableObject")]
public class SpawnableObject : ScriptableObject
{
    public GameObject prefab;
    public Vector2 spawnOffsetRange; // Random horizontal offset
    public float spawnProbability;   // 0 to 1 probability of spawning
}