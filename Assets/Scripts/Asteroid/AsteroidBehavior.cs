using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AsteroidBehavior : MonoBehaviour
{
    public float destroyY = -433f;
    public int baseHealth = 1; // Base health for asteroids
    private int sceneMultiplier = 1; // Multiplier for later scenes
    public GameObject explosionPrefab;
    private bool isDestroyed = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        // Adjust health based on scene
        AdjustHealthByScene();

        // Get the SpriteRenderer and save the original color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
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
                sceneMultiplier = 3; // Increase HP multiplier
                break;
            case "Level3":
                sceneMultiplier = 6; // Further increase HP
                break;
            default:
                sceneMultiplier = 1;
                break;
        }

        // Apply the multiplier
        baseHealth *= sceneMultiplier;
    }

    void Update()
    {
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator BlinkRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = originalColor;
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
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound);
            DestroyAsteroid();
        }
    }

    void DestroyAsteroid()
    {
        GameManager.instance.AddScore(10);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
