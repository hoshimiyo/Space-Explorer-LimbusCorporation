using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    public float speed = 400f; // Speed of the laser
    public GameObject explosionPrefab; // Assign explosion prefab in Inspector

    void Update()
    {
        // Standard laser movement (moves straight)
        transform.Translate(Vector2.up * speed * 3 * Time.deltaTime);

        // Destroy laser if it goes off-screen
        if (transform.position.y > 451)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroids"))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound);
            other.GetComponent<AsteroidBehavior>().TakeDamage(ShipStat.laserDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("BigAsteroid"))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound);
            other.GetComponent<BigAsteroidBehavior>().TakeDamage(ShipStat.laserDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            other.GetComponent<BossBehavior>().TakeDamage(ShipStat.laserDamage);
            Destroy(gameObject);
        }
    }
}
