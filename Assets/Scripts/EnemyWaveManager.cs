using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyWaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyTypes; // Different enemy prefabs
        public int enemyCount; // Number of enemies in this wave
        public float spawnRate; // Time between each enemy spawn
    }

    public Wave[] waves; // Array of waves
    public Transform spawnPoint; // Where enemies spawn
    public float minX = -700f, maxX = 700f; // Random X range
    public float timeBetweenWaves = 5f; // Delay between waves
    private int currentWaveIndex = 0;
    [SerializeField] private GameObject transitionPanel;
    public ProgressBar progressBar; 

    private int totalEnemies;
    private int defeatedEnemies;

    void Start()
    {
        transitionPanel.SetActive(false); // Hide the transition panel
        CalculateTotalEnemies();
        Debug.Log("Total :"+ totalEnemies);
        progressBar.SetMaxValue(totalEnemies); // Set the max value of the progress bar
        StartCoroutine(StartWaves());
    }

    void Update()
    {
        progressBar.SetValue(defeatedEnemies);
    }

    void CalculateTotalEnemies()
    {
        totalEnemies = 0;
        foreach (Wave wave in waves)
        {
            totalEnemies += wave.enemyCount;
        }
        Debug.Log("Total enemies: " + totalEnemies);
    }

    IEnumerator StartWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            currentWaveIndex++;
        }
        AsteroidSpawner.stopSpawner = false; // Disable spawner before transitioning
        StarScript.stopSpawner = false; // Disable spawner before transitioning
        PowerUpSpawner.stopSpawner = false; // Disable spawner before transitioning
        // Wait until all enemies are gone
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemies").Length == 0);

        // Show "Wave Cleared" notification
        Debug.Log("Finish");

        yield return new WaitForSeconds(5f); // Wait 5 sec before transition
        Time.timeScale = 0; // Pause the game
        // Transition to the next scene
        transitionPanel.SetActive(true);
    }

    IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy(wave.enemyTypes);
            yield return new WaitForSeconds(wave.spawnRate);
        }
    }

    void SpawnEnemy(GameObject[] enemyTypes)
    {
        if (enemyTypes.Length == 0) return;

        // Pick a random enemy type
        GameObject enemyPrefab = enemyTypes[Random.Range(0, enemyTypes.Length)];

        // Pick a random X position
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, spawnPoint.position.y);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Make the enemy fly down
        enemy.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, -25f);

        // Update progress bar when an enemy is defeated
        enemy.GetComponent<BaseEnemyBehavior>().OnEnemyDefeated += () =>
        {
            defeatedEnemies++;
            progressBar.SetValue(defeatedEnemies);
        };
    }

}