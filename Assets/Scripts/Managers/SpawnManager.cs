using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Private Serialized Variables
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private SpawnableObject[] spawnables;
    [SerializeField] private float spawnDistanceAhead = 10f;
    [SerializeField] private float distanceBetweenSpawns = 2f;
    #endregion

    #region Private variables
    private GameObject player;
    private float nextSpawnY;
    #endregion

    #region Monobehaviour
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
        while (player.transform.position.y + spawnDistanceAhead > nextSpawnY)
        {
            SpawnAt(nextSpawnY);
            nextSpawnY += distanceBetweenSpawns;
        }
    }

    #endregion

    #region Private Functions
    private void SpawnAt(float yPos)
    {
        foreach (var spawnable in spawnables)
        {
            if (Random.value <= spawnable.spawnProbability)
            {
                float xOffset = Random.Range(-spawnable.spawnOffsetRange.x, spawnable.spawnOffsetRange.x);
                Vector3 spawnPos = new Vector3(xOffset, yPos, 0);
                Instantiate(spawnable.prefab, spawnPos, Quaternion.identity);
            }
        }
    }
    #endregion
}
