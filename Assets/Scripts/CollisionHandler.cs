using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public GameObject explosionPrefab; // Assign explosion prefab in Inspector
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stars"))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.starPickupSound); // Pick up star sound
            GameManager.instance.AddScore(50); // Increase score by 50
            Destroy(other.gameObject); // Remove star
            Debug.Log("Star collected");
        }
        else if (other.CompareTag("Boss"))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound); // Play explosion sound
            Destroy(gameObject); // Remove player
            Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            GameManager.instance.GameOver(); // Trigger game over
            Debug.Log("skill issue");
        }
        else if (other.CompareTag("Asteroids") || other.CompareTag("BigAsteroid") || other.CompareTag("Enemies")) // Detect asteroid, big asteroid, or enemy laser
        {
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound); // Play explosion sound
            Destroy(other.gameObject); // Remove asteroid
            Destroy(gameObject); // Remove player
            Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            GameManager.instance.GameOver(); // Trigger game over
            Debug.Log("skill issue");
        }
    }
}
