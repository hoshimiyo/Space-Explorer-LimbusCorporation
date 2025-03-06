using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BossBehavior : BaseEnemyBehavior
{
    private bool entering = true;
    [SerializeField] private float stopYPosition = 271f; // Y position where the boss stops moving
    public GameObject minionPrefab;
    public ProgressBar progressBar;
    protected override void Start()
    {
        moveSpeed = 100f;
        health = 100;
        fireRate = 4f;
        scoreValue = 10000;
        EnterScreen(moveSpeed);
        base.Start();

        // Set the max value of the progress bar to the boss's initial health
        progressBar.SetMaxValue(health);

        InvokeRepeating("SpawnMinions", 5f, 10f);
    }

    protected override void Update()
    {
        if (!entering)
        {
            HoverLeftRight();
        }
    }

    public void EnterScreen(float speed)
    {
        StartCoroutine(FlyIntoScreen(speed));
    }

    IEnumerator FlyIntoScreen(float speed)
    {
        while (transform.position.y > stopYPosition)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            yield return null;
        }

        entering = false; // Boss has entered the screen
    }

    protected override void HoverLeftRight()
    {
        float hoverDistance = 380f; // Adjust how far it moves left and right
        float hoverSpeed = 1f; // Adjust how fast it moves

        float xPosition = Mathf.Sin(Time.timeSinceLevelLoad * hoverSpeed) * hoverDistance;
        transform.position = new Vector3(startPosition.x + xPosition, transform.position.y, transform.position.z);
    }

    protected override void FireLaser()
    {
        int numberOfLasers = 5; // Number of lasers fired at once
        float angleStep = 15f; // Angle difference between lasers
        float startAngle = -30f; // Starting angle for spread

        foreach (Transform firePoint in firePoints)
        {
            for (int i = 0; i < numberOfLasers; i++)
            {
                float angle = startAngle + (i * angleStep);
                Quaternion rotation = Quaternion.Euler(0, 0, angle); // Rotate the laser
                GameObject laser = Instantiate(laserPrefab, firePoint.position, rotation);

                // Set the laser's velocity in the direction of the angle
                Vector2 direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), -Mathf.Cos(angle * Mathf.Deg2Rad));
                laser.GetComponent<Rigidbody2D>().linearVelocity = direction * 5f;
            }
        }
    }

    void SpawnMinions()
    {
        Instantiate(minionPrefab, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
        Instantiate(minionPrefab, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
    }

    public override void DestroyEnemy()
    {
        CancelInvoke("SpawnMinions");
        AsteroidSpawner.stopSpawner = false; // Disable spawner before transitioning
        StarScript.stopSpawner = false; // Disable spawner before transitioning
        PowerUpSpawner.stopSpawner = false; // Disable spawner before transitioning
        base.DestroyEnemy();
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        progressBar.SetValue(health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle boss death (e.g., play animation, load next scene)
        SceneManager.LoadScene("EndGame");
    }
}