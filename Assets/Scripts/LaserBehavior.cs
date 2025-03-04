using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    public float speed = 400f; // Speed of the laser
    public GameObject explosionPrefab; // Assign explosion prefab in Inspector
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime); // Move laser upward
        if(transform.position.y > 451)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroids"))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound); // Play explosion sound
            Destroy(other.gameObject); // Destroy asteroid on hit
            Destroy(gameObject); // Destroy the laser itself
            Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            GameManager.instance.AddScore(10); // Increase score by 10
        }    
        else if (other.CompareTag("BigAsteroid")) // Detect big asteroid
        {
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound);
            other.GetComponent<BigAsteroidBehavior>().TakeDamage();
            Destroy(gameObject); // Destroy the laser
        }
    }
}
