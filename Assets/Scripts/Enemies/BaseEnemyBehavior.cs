using System.Collections;
using UnityEngine;

public class BaseEnemyBehavior : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 3f;    
    [SerializeField] protected int health = 3;
    [SerializeField] protected GameObject laserPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float fireRate = 2f;
    [SerializeField] protected bool canMove = true;
    
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        if (firePoint != null)
        {
            InvokeRepeating("FireLaser", fireRate, fireRate);
        }
    }

    protected virtual void Update()
    {
        if (canMove)
        {
            HoverLeftRight();
        }
    }

    protected virtual void HoverLeftRight()
    {
        float moveDirection = Mathf.Sin(Time.time * moveSpeed);
        transform.position += new Vector3(moveDirection * Time.deltaTime, 0, 0);
    }

    protected virtual void FireLaser()
    {
        Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
    }

    public virtual void TakeDamage()
    {
        health--;
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
