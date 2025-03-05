using System.Collections;
using UnityEngine;

public class BigAsteroidBehavior : MonoBehaviour
{
    public float destroyY = -433f;
    public int health = 3; // Takes 3 hits before being destroyed
    public GameObject explosionPrefab;
    public GameObject starPrefab; // Star prefab to spawn when destroyed
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage()
    {
        health--;
        StartCoroutine(BlinkRed());

        if (health <= 0)
        {
            DestroyAsteroid();
        }
    }

    void DestroyAsteroid()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Spawn stars at the asteroid's position
        SpawnStars(3);
        GameManager.instance.AddScore(20);
        Destroy(gameObject);
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

    void Update()
    {
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }
}
