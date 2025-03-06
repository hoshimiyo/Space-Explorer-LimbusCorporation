using UnityEngine;

public class HomingLaserBehavior : MonoBehaviour
{
    public float speed = 100f; // Speed of the laser
    public float rotateSpeed = 50f; // Rotation speed for homing lasers
    private Transform target; // Target for homing laser
    public GameObject explosionPrefab; // Assign explosion prefab in Inspector

    void Start()
    {
        FindTarget();
    }

    void Update()
    {
        if (target != null)
        {
            // Move towards the target with rotation
            Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
            direction.Normalize();

            // Adjust rotation speed
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);

            // Adjust the speed dynamically based on distance to target
            float distance = Vector2.Distance(transform.position, target.position);
            float homingSpeed = Mathf.Lerp(speed * 0.5f, speed, distance / 20f); // Gradually increase speed as it gets closer
            transform.position += transform.up * homingSpeed * Time.deltaTime;
        }
        else
        {
            // Standard laser movement (moves straight)
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }

        // Destroy laser if it goes off-screen
        if (transform.position.y > 451)
        {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroids"))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound);
            other.GetComponent<AsteroidBehavior>().TakeDamage(ShipStat.laserDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("BigAsteroid"))
        {
            AudioManager.instance.PlaySound(AudioManager.instance.explosionSound);
            other.GetComponent<BigAsteroidBehavior>().TakeDamage(ShipStat.laserDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            other.GetComponent<BossBehavior>().TakeDamage(ShipStat.laserDamage);
            Destroy(gameObject);
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemies");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroids");
        GameObject[] bigAsteroids = GameObject.FindGameObjectsWithTag("BigAsteroid");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;

        // Check enemies
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = enemy;
            }
        }

        // Check asteroids
        foreach (GameObject asteroid in asteroids)
        {
            float distance = Vector2.Distance(transform.position, asteroid.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = asteroid;
            }
        }

        // Check big asteroids
        foreach (GameObject bigAsteroid in bigAsteroids)
        {
            float distance = Vector2.Distance(transform.position, bigAsteroid.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = bigAsteroid;
            }
        }

        // Check asteroids
        foreach (GameObject boss in bosses)
        {
            float distance = Vector2.Distance(transform.position, boss.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTarget = boss;
            }
        }

        // Assign the closest found target
        if (nearestTarget != null)
        {
            target = nearestTarget.transform;
        }
    }

}
