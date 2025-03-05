using UnityEngine;

public class EnemyShooter : BaseEnemyBehavior
{
    private Transform player;
    private Rigidbody2D laserRb;

    protected override void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        scoreValue = 300;
        fireRate = 2.5f;
        base.Start();
    }

    protected override void FireLaser()
    {
        if (player == null) return; 

        foreach (Transform firePoints in firePoints)
        {
            // Get direction from enemy to player
            Vector3 direction = player.position - transform.position;

            // Create laser
            GameObject laser = Instantiate(laserPrefab, firePoints.position, Quaternion.identity);

            laserRb = laser.GetComponent<Rigidbody2D>();
            laserRb.linearVelocity = new Vector2(direction.x, direction.y).normalized;

            // Rotate laser to face player
            float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            laser.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            // Move laser towards player
            laser.GetComponent<Rigidbody2D>().linearVelocity = direction * 0.5f;
        }
    }
}
