using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigAsteroidBehavior : MonoBehaviour
{
    public float destroyY = -433f;
    public int baseHealth = 3; // Takes 3 hits before being destroyed
    private int sceneMultiplier = 1; // Multiplier for later scenes
    public GameObject explosionPrefab;
    public GameObject starPrefab; // Star prefab to spawn when destroyed
    public GameObject powerUpPrefab;
    private bool isDestroyed = false; // Prevent multiple destroy calls
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        // Adjust health based on scene
        AdjustHealthByScene();

        // Get the SpriteRenderer and save the original colors
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;

        baseHealth = baseHealth - damage;
        StartCoroutine(BlinkRed());

        if (baseHealth <= 0)
        {
            isDestroyed = true;
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound); // Play explosion sound
            DestroyAsteroid();
        }
    }

    void AdjustHealthByScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Gameplay":
                sceneMultiplier = 1;
                break;
            case "Level2":
                sceneMultiplier = 2; // Increase HP multiplier
                break;
            case "Level3":
                sceneMultiplier = 3; // Further increase HP
                break;
            default:
                sceneMultiplier = 1;
                break;
        }

        // Apply the multiplier
        baseHealth *= sceneMultiplier;
    }

    void DestroyAsteroid()
    {
        GameManager.instance.AddScore(20);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);

        // Spawn stars at the asteroid's position
        SpawnStars(3);

        // 20% chance to spawn a power-up
        if (Random.value <= 0.2f)
        {
            SpawnPowerUp();
        }
    }

    void SpawnStars(int count)
    {
        float spawnRadius = 100f; // Adjust this value to control spread

        for (int i = 0; i < count; i++)
        {
            // Generate a random position in a circular area around the asteroid
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;

            Instantiate(starPrefab, (Vector2)transform.position + randomOffset, Quaternion.identity);
        }
    }

    IEnumerator BlinkRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red; // Change color to red
            yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds
            spriteRenderer.color = originalColor; // Revert back to original color
        }
    }

    void SpawnPowerUp()
    {
        if (powerUpPrefab != null)
        {
            GameObject powerUpInstance = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);

            // Assign a random power-up type
            PowerUp powerUpScript = powerUpInstance.GetComponent<PowerUp>();
            if (powerUpScript != null)
            {
                powerUpScript.powerUpType = (PowerUp.PowerUpType)Random.Range(0, 3); // Random type
                powerUpScript.SetPowerUpColor(); // Apply correct color
            }
        }
    }
    void Update()
    {
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
