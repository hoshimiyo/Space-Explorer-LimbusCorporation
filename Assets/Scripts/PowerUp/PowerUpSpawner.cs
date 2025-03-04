using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public float spawnInterval = 10f;
    public float speed = 20f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPowerUp();
            timer = 0;
        }
    }

    void SpawnPowerUp()
    {
        // Randomize the spawn position at the top of the screen
        Vector2 spawnPosition = new Vector2(
            Random.Range(-700, 700),
            Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.1f, 0)).y // Slightly above the camera
        );

        GameObject present = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        // Assign movement downward
        Rigidbody2D rb = present.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, -speed);
        }

        PowerUp powerUpScript = present.GetComponent<PowerUp>();
        powerUpScript.powerUpType = (PowerUp.PowerUpType)Random.Range(0, 3);
    }
}
