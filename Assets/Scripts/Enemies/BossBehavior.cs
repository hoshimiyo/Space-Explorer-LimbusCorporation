using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BossBehavior : BaseEnemyBehavior
{
    private bool entering = true;
    [SerializeField] private float stopYPosition = 271f; // Y position where the boss stops moving
    public GameObject minionPrefab;
    public ProgressBar progressBar;
    [SerializeField] private Transform[] explosionLocation;
    [SerializeField] private TextMeshProUGUI victoryTitle;
    [SerializeField] private Canvas canvas;
    private RectTransform healthBarRect;
    protected override void Start()
    {
        if (progressBar != null)
            healthBarRect = progressBar.GetComponent<RectTransform>();


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

        if (transform != null && canvas != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, -340f, 0));
            healthBarRect.position = screenPos;
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
        StartCoroutine(HandleDestruction());
        GameManager.instance.AddScore(scoreValue);
    }

    IEnumerator HandleDestruction()
    {
        Invoke(nameof(TransitionToNextScene), 10f); // Call transition BEFORE destroying the enemy

        yield return StartCoroutine(PrepareDestroy()); // Wait for explosions to finish


        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        progressBar.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        victoryTitle.gameObject.SetActive(true);
    }

    void TransitionToNextScene()
    {
        SceneManager.LoadScene("EndGame");
    }

    IEnumerator PrepareDestroy()
    {
        CancelInvoke("SpawnMinions");
        CancelInvoke("HoverLeftRight");
        CancelInvoke("FireLaser");

        // Disable spawners
        AsteroidSpawner.stopSpawner = true; // ✅ Disable spawner before transitioning
        StarScript.stopSpawner = true; // ✅ Disable spawner before transitioning
        PowerUpSpawner.stopSpawner = true; // ✅ Disable spawner before transitioning

        // Disable movement and collisions
        this.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        this.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        this.GetComponent<Collider2D>().enabled = false;

        for (int a = 0; a < 10; a++)
        {
            foreach (Transform location in explosionLocation)
            {
                Instantiate(explosionPrefab, location.position, Quaternion.identity);
            }
            StartCoroutine(BlinkRed());
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(BlinkRed());
        AudioManager.instance.PlaySound(AudioManager.instance.enemyTakeDamageSound);
        progressBar.SetValue(health);

        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

}