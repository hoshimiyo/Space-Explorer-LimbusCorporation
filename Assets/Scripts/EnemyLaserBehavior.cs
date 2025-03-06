using UnityEngine;

public class EnemyLaserBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 400f; // Speed of the laser
    public GameObject explosionPrefab; // Assign explosion prefab in Inspector
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime); // Move laser downward
        if (transform.position.y < -455)
        {
            Destroy(gameObject);
        }
    }
}
