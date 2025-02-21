using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public GameObject explosionPrefab; // Assign explosion prefab in Inspector
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stars"))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.starPickupSound); // Pick up star sound
            GameManager.instance.AddScore(50 * (int) Time.fixedTime); // Increase score by 10
            Destroy(other.gameObject); // Remove star
            Debug.Log("Star collected");
        }
        else if (other.CompareTag("Asteroids"))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound); // Play explosion sound
            Destroy(other.gameObject); // Remove asteroid
            Destroy(gameObject); // Remove player
            Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            GameManager.instance.GameOver(); // Trigger game over
            Debug.Log("trol");
        }
    }
}
