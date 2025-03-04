using UnityEngine;

public class BossBehavior : BaseEnemyBehavior
{
    // public GameObject minionPrefab;

    protected override void Start()
    {
        moveSpeed = 2f;
        health = 10;
        fireRate = 1f;
        base.Start();

        // InvokeRepeating("SpawnMinions", 5f, 10f);
    }

    protected override void FireLaser()
    {
        float angleStep = 15f;
        for (int i = -1; i <= 1; i++)
        {
            GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(i * angleStep, -5f);
        }
    }

    // void SpawnMinions()
    // {
    //     Instantiate(minionPrefab, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
    //     Instantiate(minionPrefab, transform.position + new Vector3(1, 0, 0), Quaternion.identity);
    // }
}
