public static class ShipStat
{
    //Ship stats
    public static float moveSpeed = 335f;
    public static float fireRate = 0.5f;
    public static float fireRateMultiplier = 1f;
    public static int laserFireRateLevel = 0;
    public static float[] multipliers = { 1f, 0.8f, 0.6f, 0.4f};
    public static int laserCount = 1;
    public static float laserSpreadAngle = 10f;
    public static int health = 3;
    public static int healthCap = 3;
    public static int laserDamage = 1;

    // Overloading state
    public static bool overloadingState = false;
    public static float overloadingStateDuration = 3f;
    public static float overloadingMultiplier = 0.05f;

    // Invincibility frames (iFrame)
    public static bool iFrame = false;
    public static float iFrameDuration = 2f;
    public static float iFramePowerUpDuration = 3f;

    // Overkill state (lmao im bad at naming stuff)
    public static bool overkillState = false; // New state for homing burst mode
    public static float overkillStateDuration = 5f; // Seeker mode lasts 5 seconds
    public static int homingLaserCount = 3; // Number of homing lasers in seeker mode
    public static float homingLaserFireRate = 0.5f;
    public static float homingLaserSpreadAngle = 20f;


    public static void ResetStat()
    {
        moveSpeed = 335f;
        fireRate = 0.5f;
        fireRateMultiplier = 1f;
        laserFireRateLevel = 0;
        laserCount = 1;
        laserSpreadAngle = 10f;
        health = 3;
        overloadingState = false;
        iFrame = false;
    }
}
