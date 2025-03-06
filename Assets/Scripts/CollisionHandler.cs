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
            Destroy(gameObject); // Remove player
            Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
            GameManager.instance.GameOver(); // Trigger game over
            Debug.Log("skill issue");
        }
        else if (other.CompareTag("Asteroids")) // Detect asteroid, big asteroid, or enemy laser
        {
            other.GetComponent<AsteroidBehavior>().TakeDamage(3);
            GetComponent<ShipControl>().TakeDamage(1);
        }
        else if (other.CompareTag("BigAsteroid"))
        {
            other.GetComponent<BigAsteroidBehavior>().TakeDamage(3);
            GetComponent<ShipControl>().TakeDamage(3);
            Debug.Log("skill issue");
        }
        else if (other.CompareTag("Enemies"))
        {
            other.GetComponent<BaseEnemyBehavior>().DestroyEnemy();
            GetComponent<ShipControl>().TakeDamage(1);    
        }
        else if (other.CompareTag("EnemyLaser"))
        {
            Destroy(other.gameObject); // Remove enemy laser
            GetComponent<ShipControl>().TakeDamage(1);
        }
    }
}
