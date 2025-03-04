using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public float moveSpeed = 335f;
    private Vector2 screenBounds;
    private float shipWidth, shipHeight;
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;
    public float fireRate = 0.5f; // Delay between shots
    public float baseFireRate = 0.5f;
    private bool canShoot = true;
    private int laserFireRateLevel = 0;
    private List<LaserBehavior> activeLasers = new List<LaserBehavior>(); // List to track active lasers
    private bool overloadingState = false; // To check if auto shooting is active
    private float overloadingStateDuration = 5f; // Duration of auto shooting
    private float overloadingStateTimer = 0f; // Timer to track auto shooting duration
    private float laserSpreadAngle = 10f; // Angle range for laser spread, in degrees
    private int laserCount = 1; // Number of lasers 
    private float lastSoundTime = 0f; // Time when the sound was last played
    private float soundCooldown = 0.1f; // Cooldown between sounds (in seconds)


    void Start()
    {
        // Get the screen bounds in world coordinates
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Get spaceship size
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        shipWidth = sr.bounds.extents.x; // Half of the width
        shipHeight = sr.bounds.extents.y; // Half of the height
    }


    void Update()
    {
        MoveShip();

        if (!overloadingState && canShoot)
        {
            StartCoroutine(ShootWithDelay());
        }
        // If auto shooting is active, fire continuously
        if (overloadingState)
        {
            overloadingStateTimer -= Time.deltaTime;

            if (overloadingStateTimer > 0f)
            {
                if (canShoot)
                {
                    StartCoroutine(ShootWithDelay());
                }
            }
            else
            {
                // End auto shooting after 10 seconds
                overloadingState = false;
                fireRate = baseFireRate; // Revert to original fire rate after 10 seconds
                Debug.Log("Overloading ended");
            }
        }
    }

    IEnumerator ShootWithDelay()
    {
        canShoot = false;
        ShootLaser();
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    void MoveShip()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);

        // Clamp position within screen bounds
        newPosition.x = Mathf.Clamp(newPosition.x, -screenBounds.x + shipWidth, screenBounds.x - shipWidth);
        newPosition.y = Mathf.Clamp(newPosition.y, -screenBounds.y + shipHeight, screenBounds.y - shipHeight);

        transform.position = newPosition;
    }
    void ShootLaser()
    {
        // Prevent sound from being played too frequently
        if (Time.time - lastSoundTime >= soundCooldown)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.laserSound);
            lastSoundTime = Time.time; // Update the last sound play time
        }

        // Fire a laser with a random spread when in overloading state
        if (overloadingState)
        {
            for (int i = 0; i < laserCount; i++)
            {

                float randomAngle = UnityEngine.Random.Range(-laserSpreadAngle, laserSpreadAngle); // Generate random angle within the spread range
                Quaternion rotation = laserSpawnPoint.rotation * Quaternion.Euler(0, 0, randomAngle);
                Instantiate(laserPrefab, laserSpawnPoint.position, rotation);
            }
        }
        else
        {
            // Fire lasers with an even spread when not in overloading state
            if (laserCount > 1)
            {
                float spreadStep = laserSpreadAngle / (laserCount - 1); // Calculate the angle step for each laser
                for (int i = 0; i < laserCount; i++)
                {
                    // Calculate the angle for each laser evenly distributed
                    float evenAngle = -laserSpreadAngle / 2 + spreadStep * i; // Evenly distribute lasers within the range
                    Quaternion rotation = laserSpawnPoint.rotation * Quaternion.Euler(0, 0, evenAngle);
                    Instantiate(laserPrefab, laserSpawnPoint.position, rotation);

                }
            }
            //Fire a single laser with no spread 
            else Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation);
        }
    }

    internal void LaserFireRateUp()
    {
        if (laserFireRateLevel <= 3)
        {
            // Increase fire rate up to 75% max reduction
            float newFireRate = fireRate * 0.75f;
            fireRate = Mathf.Max(newFireRate, fireRate * 0.25f);
            baseFireRate = fireRate;
            laserFireRateLevel++;
            Debug.Log("Laser firerate increased to: " + fireRate);
        }

        // On the 4th pickup, enable piercing shots for 10 seconds
        if (laserFireRateLevel >= 4)
        {
            //fireRate = baseFireRate * 0.5f; // 200% fire rate (half the delay time)
            Debug.Log("Max Fire rate reached, overloading begin");

            if (!overloadingState)
            {
                overloadingState = true;
                overloadingStateTimer = overloadingStateDuration; // Reset the auto-shoot duration to full
                fireRate = baseFireRate * 0.1f; // Set fire rate to 1000% ?

            }
            else
            {
                overloadingStateTimer = overloadingStateDuration;
                Debug.Log("Overloading refreshed");
            }
        }
    }

    internal void ActivateShield()
    {
        Debug.Log("Shield power-up activated");
    }

    internal void StartLaserSpread()
    {
        if (laserCount < 5)
        {
            laserCount += 2;
            Debug.Log("Laser spread power-up activated");
        }
        else Debug.Log("Laser spread maxed");
    }
}