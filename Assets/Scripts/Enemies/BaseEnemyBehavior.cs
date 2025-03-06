using System.Collections;
using UnityEngine;

public class BaseEnemyBehavior : MonoBehaviour
{
    protected float moveSpeed = 3f;
    protected int health = 3;
    [SerializeField] protected GameObject laserPrefab;
    [SerializeField] protected Transform[] firePoints;
    protected float fireRate = 2f;
    protected Vector3 startPosition;
    [SerializeField] protected GameObject explosionPrefab;
    protected int scoreValue = 10;

    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    protected virtual void Start()
    {
        startPosition = transform.position; // Save the initial position
        if (firePoints != null)
        {
            InvokeRepeating("FireLaser", fireRate, fireRate);
        }
    }

    protected virtual void Update()
    {
        if (transform.position.y < -455)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void HoverLeftRight()
    {
        float hoverDistance = 3f; // Adjust how far it moves left and right
        float hoverSpeed = 2f; // Adjust how fast it moves

        float xPosition = Mathf.Sin(Time.time * hoverSpeed) * hoverDistance;
        transform.position = new Vector3(startPosition.x + xPosition, transform.position.y, transform.position.z);
    }


    protected virtual void FireLaser()
    {
        foreach (Transform firePoint in firePoints)
            Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(BlinkRed());

        if (health <= 0)
            DestroyEnemy();
    }

    IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    protected virtual void DestroyEnemy()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }
}
