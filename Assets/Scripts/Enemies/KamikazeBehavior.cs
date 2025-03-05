using UnityEngine;

public class KamikazeBehavior : BaseEnemyBehavior
{
    private Transform player;

    protected override void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        scoreValue = 100;
        moveSpeed = 200f;
        health = 1; // One hit kill
        base.Start();
    }

    protected override void Update()
    {
        if (player != null)
        {
            // Get direction from enemy to player
            Vector3 direction = player.position - transform.position;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(direction.x, direction.y).normalized;

            // Rotate to face player
            float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            rb.GetComponent<Rigidbody2D>().linearVelocity = direction * 2f;
        }
    }
}
