using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;  // Reference to the asteroid prefab
    public GameObject bigAsteroidPrefab;
    public float spawnInterval = 4f;   // Initial time between asteroid spawns
    public float speedIncreaseRate = 20f; // How much speed increases over time
    public float spawnRateIncrease = 0.5f; // How much spawn rate increases over time
    public float maxSpeed = 200f;       // Maximum falling speed
    public float minSpeed = 80f;        // Initial falling speed
    public float bigAsteroidChance = 0.2f; // 20% chance to spawn a big asteroid
    [SerializeField]
    private float currentSpeed;   // Current speed of falling asteroids
    [SerializeField]
    private float currentInterval; // Current spawn interval

    void Start()
    {
        currentSpeed = minSpeed;
        currentInterval = spawnInterval;
        InvokeRepeating("SpawnAsteroid", 0f, currentInterval); // Start spawning asteroids
        InvokeRepeating("IncreaseDifficulty", 5f, 5f); // Increase difficulty over time
    }

    void SpawnAsteroid()
    {
        // Randomize the spawn position at the top of the screen
        Vector2 spawnPosition = new Vector2(
            Random.Range(-700, 700),
            Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.1f, 0)).y // Slightly above the camera
        );


        if (Random.value < bigAsteroidChance)
        {
            GameObject bigAsteroid = Instantiate(bigAsteroidPrefab, spawnPosition, Quaternion.identity);
            // Assign movement downward
            Rigidbody2D rb = bigAsteroid.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(0, -currentSpeed);
            }
        }
        else
        {
            // Instantiate the asteroid
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
            // Assign movement downward
            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(0, -currentSpeed);
            }
        }

    }

    void IncreaseDifficulty()
    {
        // Increase speed, but don't exceed maxSpeed
        currentSpeed = Mathf.Min(currentSpeed + speedIncreaseRate, maxSpeed);

        // Decrease spawn interval (faster spawning), but limit how fast it can go
        currentInterval = Mathf.Max(0.5f, currentInterval - spawnRateIncrease);

        bigAsteroidChance = Mathf.Min(0.5f, bigAsteroidChance + 0.03f);

        // Restart asteroid spawning with updated interval
        CancelInvoke("SpawnAsteroid");
        InvokeRepeating("SpawnAsteroid", 0f, currentInterval);
    }
}
