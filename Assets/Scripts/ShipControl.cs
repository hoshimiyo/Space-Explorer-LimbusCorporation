using System;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public float moveSpeed = 335f;
    private Vector2 screenBounds;
    private float shipWidth, shipHeight;
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootLaser();
        }
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
        AudioManager.instance.PlaySound(AudioManager.instance.laserSound);
        Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation);
    }

    internal void LaserPenetrate()
    {
        Debug.Log("Laser penetrate power-up activated");
    }

    internal void ActivateShield()
    {
        Debug.Log("Shield power-up activated");
    }

    internal void StartLaserSpread()
    {
        Debug.Log("Laser spread power-up activated");
    }
}
