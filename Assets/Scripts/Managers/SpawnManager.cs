using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Private Serialized Variables
    [Header("Layer Mask")]
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private LayerMask spawnBlockingLayer;

    [Space(5)]
    [Header("Spawn Data")] 
    [SerializeField] private GameObject swapnableObjectsContainer;
    [SerializeField] private SpawnableObject[] spawnables;
    [SerializeField] private float spawnDistanceAhead = 10f;
    [SerializeField] private float distanceBetweenSpawns = 2f;
    [SerializeField] private List<Transform> atPlatformSpawnPositions = new List<Transform>();

    [Space(5)]
    [Header("Player")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject playerPrefab;
    #endregion

    #region Private variables
    private GameObject player;
    private float nextSpawnY;
    #endregion

    #region Monobehaviour
    /// <summary>
    /// Spawn player
    /// </summary>
    private void Awake()
    {
        player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }
    private void Start()
    {
        nextSpawnY = player.transform.position.y + distanceBetweenSpawns;
    }

    private void Update()
    {
        // Spawn objects at a spawnDistanceAhead from player, and check it player has crossed it then spawn again.
        while (player.transform.position.y + spawnDistanceAhead > nextSpawnY)
        {
            SpawnAt(nextSpawnY);
            nextSpawnY += distanceBetweenSpawns;
        }
    }

    #endregion

    #region Private Functions
    /// <summary>
    /// Go through the list of spawnables serialized objects, based on there probability of spawning it spawns them
    /// Goes through a for loop inside, with a defined number of attempts to spawn a spawnable object, and breaks the loop if object get's spawned
    /// plus also checks if a spawnable require platform to spawn, in that case calls CheckForNearestSpawnPosition() and spawn at that pos.
    /// Uses randome.range and x offset define in SO to spawn the object in x axis randomly.
    /// do a last check using Physics2D.OverlapBox to make sure the objects don't get spawned on each other or on spawnBlockingLayer.
    /// </summary>
    /// <param name="yPos"></param>
    private void SpawnAt(float yPos)
    {
        foreach (var spawnable in spawnables)
        {
            if (Random.value <= spawnable.spawnProbability)
            {
                int maxAttempts = 25;
                bool spawned = false;
                Vector2 prefabSize = GetWorldSize(spawnable.prefab);

                if (!spawnable.requiresPlatform)
                {
                    spawnBlockingLayer |= platformLayer; // Add platform only if NOT required
                }

                for (int i = 0; i < maxAttempts && !spawned; i++)
                {
                    float xOffset = Random.Range(-spawnable.spawnOffsetRange.x, spawnable.spawnOffsetRange.x);
                    Vector3 spawnPos = new Vector3(xOffset, yPos, 0);

                    // If object requires platform, we check the predefined nearest position on platform 
                    if (spawnable.requiresPlatform)
                    {
                        Transform nearest = CheckForNearestSpawnPosition(atPlatformSpawnPositions, yPos);

                        if (nearest == null) 
                            continue;

                        spawnPos = nearest.position;
                    }

                    // Check for overlap with spawn-blocking layer (platform or something else if required)
                    Collider2D hitCollider = Physics2D.OverlapBox(spawnPos, prefabSize, 0f, spawnBlockingLayer);
                    if (hitCollider == null)
                    {
                        GameObject obj  = Instantiate(spawnable.prefab, spawnPos, Quaternion.identity);
                        obj.transform.SetParent(swapnableObjectsContainer.transform);
                        spawned = true;
                    }
                }

                if (!spawned)
                {
                    Debug.LogWarning($"Couldn't spawn {spawnable.prefab.name} at Y={yPos} without overlap.");
                }
            }
        }
    }

    /// <summary>
    /// Check for nearest pre defined spawn position on platform, from the atPlatformSpawnPositions
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="targetY"></param>
    /// <returns></returns>
    private Transform CheckForNearestSpawnPosition(List<Transform> positions, float targetY)
    {
        Transform nearest = null;
        float smallestDiff = float.MaxValue;

        foreach (var pos in positions)
        {
            float diff = Mathf.Abs(pos.position.y - targetY);
            if (diff < smallestDiff)
            {
                smallestDiff = diff;
                nearest = pos;
            }
        }

        return nearest;
    }
    /// <summary>
    /// Gets actual object size based on it's local scale and collider
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private Vector2 GetWorldSize(GameObject prefab)
    {
        BoxCollider2D collider = prefab.GetComponent<BoxCollider2D>();
        if (collider == null) return Vector2.zero;

        Vector2 size = collider.size;
        Vector2 scale = prefab.transform.localScale;

        return new Vector2(size.x * scale.x, size.y * scale.y);
    }
    #endregion
}
