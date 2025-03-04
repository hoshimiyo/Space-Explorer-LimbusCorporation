using System.Threading.Tasks;
using UnityEngine;

public class StarScript : MonoBehaviour
{
    public GameObject starPrefab; // Reference to the star prefab
    public float spawnInterval = 8f; // Time interval between spawn checks
    public int initialDelay = 5; // Initial delay before spawning starts
    void Start()
    {
        InvokeRepeating("TrySpawnStar", initialDelay, spawnInterval);
    }

    void TrySpawnStar()
    {
        if (Random.value < 0.5f) // 50% chance to spawn a star
        {
            SpawnStar();
        }
    }

    void SpawnStar()
    {
        // Random spawn position within the spawn area
        Vector2 spawnPosition = new Vector2(
            Random.Range(-649, 642),
            Random.Range(-333, 344)
        );

        Instantiate(starPrefab, spawnPosition, Quaternion.identity);
        AudioManager.instance.PlaySound(AudioManager.instance.starSound);
    }
}
