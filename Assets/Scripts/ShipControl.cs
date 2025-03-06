using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public float moveSpeed = 500f;
    private Vector2 screenBounds;
    private float shipWidth, shipHeight;
    public GameObject laserPrefab;
    public GameObject laserPrefab2;
    public Transform laserSpawnPoint;
    private bool canShoot = true;
    private bool canShootHoming = true;
    private List<LaserBehavior> activeLasers = new List<LaserBehavior>(); // List to track active lasers
    private float overloadingStateTimer = 0f; // Timer to track overloading duration
    public int laserCount = 1; // Number of lasers 
    private float lastSoundTime = 0f; // Time when the sound was last played
    private float soundCooldown = 0.1f; // Cooldown between sounds (in seconds)
    private float iFrameTimer = 0f;
    private float overkillStateTimer = 0f; // Timer to track seeker mode duration
    private SpriteRenderer shipRenderer; // Reference to the SpriteRenderer
    private Color originalColor;
    private Coroutine blinkCoroutine; // Coroutine reference for blinking effect


    void Start()
    {
        //health = PlayerStats.playerStat.health;
        //iFrameDuration = PlayerStats.playerStat.iFrameDuration;
        // Get the screen bounds in world coordinates
        Camera mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Get spaceship size
        shipRenderer = GetComponent<SpriteRenderer>();
        shipWidth = shipRenderer.bounds.extents.x; // Half of the width
        shipHeight = shipRenderer.bounds.extents.y; // Half of the height
        if (shipRenderer != null)
        {
            originalColor = shipRenderer.color;
        }
    }


    void Update()
    {
        MoveShip();
        if (canShoot) StartCoroutine(ShootWithDelay());

        // If auto shooting is active, fire continuously
        if (ShipStat.overloadingState)
        {
            overloadingStateTimer -= Time.deltaTime;
            if (overloadingStateTimer <= 0f)
            {
                ShipStat.overloadingState = false;
                ShipStat.fireRateMultiplier = ShipStat.multipliers[ShipStat.laserFireRateLevel]; // Revert to original fire rate
                Debug.Log("Overloading ended");
            }
        }
        // Update invulnerability timer if the ship is invulnerable
        if (ShipStat.iFrame)
        {
            iFrameTimer -= Time.deltaTime;
            if (iFrameTimer <= 0f)
            {
                StopBlinking();
                ShipStat.iFrame = false; // Reset invulnerability after the time has passed
                Debug.Log("Iframe ended");

            }
        }
        if (ShipStat.overkillState)
        {
            if (canShootHoming) StartCoroutine(ShootHomingLaserWithDelay());
            overkillStateTimer -= Time.deltaTime;
            if (overkillStateTimer <= 0f)
            {
                ShipStat.overkillState = false;
                Debug.Log("Overkill mode ended.");
                StopCoroutine(ShootHomingLaserWithDelay());
            }
        }
    }
    // Blinking effect while the ship is invulnerable
    private void StartBlinkingRed()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }

    private void StartShielding()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(ShieldingShip());
    }

    private void StopBlinking()
    {
        // Ensure ship is fully visible when blinking stops
        shipRenderer.enabled = true;
        shipRenderer.color = originalColor;
    }

    private IEnumerator BlinkCoroutine()
    {
        while (ShipStat.iFrame)
        {
            shipRenderer.color = Color.red;
            shipRenderer.enabled = !shipRenderer.enabled; // Toggle visibility
            yield return new WaitForSeconds(0.1f); // Blink interval
        }
    }

    private IEnumerator ShieldingShip()
    {
        while (ShipStat.iFrame)
        {
            // Alternate between the shield color and original color
            shipRenderer.color = new Color(0f / 255f, 0f / 255f, 255f / 255f);
            yield return null;
        }
    }

    IEnumerator ShootWithDelay()
    {
        canShoot = false;
        ShootLaser();
        yield return new WaitForSeconds(ShipStat.fireRate * ShipStat.fireRateMultiplier);
        canShoot = true;
    }

    IEnumerator ShootHomingLaserWithDelay()
    {
        canShootHoming = false;
        ShootHomingLaser();
        yield return new WaitForSeconds(ShipStat.homingLaserFireRate);
        canShootHoming = true;
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
        if (ShipStat.overloadingState)
        {
            for (int i = 0; i < 3; i++)
            {
                float randomAngle = UnityEngine.Random.Range(-ShipStat.laserSpreadAngle, ShipStat.laserSpreadAngle); // Generate random angle within the spread range
                Quaternion rotation = laserSpawnPoint.rotation * Quaternion.Euler(0, 0, randomAngle);
                Instantiate(laserPrefab, laserSpawnPoint.position, rotation);
            }
        }
        else
        {
            // Fire lasers with an even spread when not in overloading state
            if (ShipStat.laserCount > 1)
            {
                float spreadStep = ShipStat.laserSpreadAngle / (ShipStat.laserCount - 1); // Calculate the angle step for each laser
                for (int i = 0; i < ShipStat.laserCount; i++)
                {
                    // Calculate the angle for each laser evenly distributed
                    float evenAngle = -ShipStat.laserSpreadAngle / 2 + spreadStep * i; // Evenly distribute lasers within the range
                    Quaternion rotation = laserSpawnPoint.rotation * Quaternion.Euler(0, 0, evenAngle);
                    Instantiate(laserPrefab, laserSpawnPoint.position, rotation);
                }
            }
            //Fire a single laser with no spread 
            else Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation);
        }
    }
    void ShootHomingLaser()
    {
        // Fire homing lasers with an even spread (only during overkill mode)
        float spreadStep = ShipStat.homingLaserSpreadAngle / (ShipStat.homingLaserCount - 1); // Spread for homing lasers
        for (int i = 0; i < ShipStat.homingLaserCount; i++)
        {
            // Calculate the angle for each homing laser evenly distributed
            float evenAngle = -ShipStat.homingLaserSpreadAngle / 2 + spreadStep * i;
            Quaternion rotation = laserSpawnPoint.rotation * Quaternion.Euler(0, 0, evenAngle);
            Instantiate(laserPrefab2, laserSpawnPoint.position, rotation);
        }
    }

    internal void LaserFireRateUp()
    {
        if (ShipStat.laserFireRateLevel < 3)
        {
            ShipStat.laserFireRateLevel++;
            ShipStat.fireRateMultiplier = ShipStat.multipliers[ShipStat.laserFireRateLevel];
            Debug.Log("Laser firerate increased to level: " + ShipStat.laserFireRateLevel);
        }

        // On the 4th pickup, enable overloading
        else
        {
            Debug.Log("Max Fire rate reached, overloading begin");

            if (!ShipStat.overloadingState)
            {
                ShipStat.overloadingState = true;
                overloadingStateTimer = ShipStat.overloadingStateDuration; // Reset the duration to full
                ShipStat.fireRateMultiplier = ShipStat.overloadingMultiplier;
            }
            else
            {
                overloadingStateTimer = ShipStat.overloadingStateDuration;
                Debug.Log("Overloading refreshed");
            }
        }
    }

    internal void ActivateShield()
    {
        if (ShipStat.healthCap > ShipStat.health)
        {
            ShipStat.health += 1;
            Debug.Log("Ship healed to " + ShipStat.health);
        }

        if (!ShipStat.iFrame)
        {
            ShipStat.iFrame = true;
            iFrameTimer = ShipStat.iFramePowerUpDuration;
            Debug.Log("Shield granted. Enjoy 3s of iFrame");
            StartShielding();
        }
        else
        {
            iFrameTimer = ShipStat.iFramePowerUpDuration;
            Debug.Log("3 more sec of iFrame");
        }
    }

    internal void StartLaserSpread()
    {
        if (ShipStat.laserCount < 5)
        {
            ShipStat.laserCount += 2;
            Debug.Log($"Laser now spread to {ShipStat.laserCount} shots");
        }
        else
        {
            // Enter Overkill mode (seeker laser activation)
            if (!ShipStat.overkillState)
            {
                ShipStat.overkillState = true;
                overkillStateTimer = ShipStat.overkillStateDuration;
                Debug.Log("Max Laser spread reached. Overkill time");
            }
            else
            {
                overkillStateTimer = ShipStat.overkillStateDuration; // Refresh duration
                Debug.Log("Overkill mode refreshed");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (ShipStat.iFrame) return;
        if (ShipStat.laserCount > 1) ShipStat.laserCount -= 2;
        if (ShipStat.laserFireRateLevel > 0) ShipStat.fireRateMultiplier = ShipStat.multipliers[ShipStat.laserFireRateLevel - 1];

        ShipStat.health = ShipStat.health - damage;
        //PlayerStats.playerStat.health = health; // Update GameManager's shipHealthPoint

        if (ShipStat.health <= 0)
        {
            Destroy(gameObject); // Destroy the player ship
            Instantiate(GameManager.instance.explosionPrefab, gameObject.transform.position, Quaternion.identity); // Explosion effect
            GameManager.instance.GameOver(); // Trigger game over
        }
        else
        {
            // Trigger invulnerability for 2 seconds after taking damage
            ShipStat.iFrame = true;
            Debug.Log("IFrame on, HP at: " + ShipStat.health);
            iFrameTimer = ShipStat.iFrameDuration;
            StartBlinkingRed();
        }
    }
}