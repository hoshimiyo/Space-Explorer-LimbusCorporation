using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float destroyY = -433f;
    public enum PowerUpType { LaserSpread, Shield, LaserPenetrate }
    public PowerUpType powerUpType;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetPowerUpColor(powerUpType, spriteRenderer);
    }

    void Update()
    {
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }

    public void SetPowerUpColor(PowerUpType _powerUpType, SpriteRenderer _spriteRenderer)
    {
        switch (_powerUpType)
        {
            case PowerUpType.LaserSpread:
                _spriteRenderer.color = Color.green; // Green for speed
                break;
            case PowerUpType.Shield:
                _spriteRenderer.color = Color.blue; // Blue for shield
                break;
            case PowerUpType.LaserPenetrate:
                _spriteRenderer.color = Color.red; // Red for laser upgrade
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyPowerUp(other.gameObject);
            Destroy(gameObject); // Remove the power-up after collection
        }
        else
        {
            Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
        } 
    }

    void ApplyPowerUp(GameObject ship)
    {
        ShipControl shipControl = ship.GetComponent<ShipControl>();

        switch (powerUpType)
        {
        case PowerUpType.LaserSpread:
                shipControl.StartLaserSpread();
                break;
            case PowerUpType.Shield:
                shipControl.ActivateShield();
                break;
            case PowerUpType.LaserPenetrate:
                shipControl.LaserFireRateUp();
                break;
        }
    }
}
