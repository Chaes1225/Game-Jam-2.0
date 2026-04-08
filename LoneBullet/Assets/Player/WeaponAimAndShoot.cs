using UnityEngine;

public class WeaponAimAndShoot : MonoBehaviour
{
    [Header("Aiming References")]
    [SerializeField] private SpriteRenderer playerSprite; // Drag your PlayerSprite here
    [SerializeField] private SpriteRenderer gunSprite;    // Drag your GunSprite here

    [Header("Shooting References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private Camera mainCam;
    private Vector2 mousePosition;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        AimGun();

        mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // 2. Listen for the left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void AimGun()
    {
        // 1. Get Mouse Position
        Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // 2. Calculate Angle to Mouse
        Vector2 aimDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // 3. Rotate the Weapon Pivot
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 4. Flip the Player and Gun sprites based on mouse position
        if (mousePosition.x < transform.position.x)
        {
            // Mouse is to the left: Flip player left, flip gun Y so it isn't upside down
            playerSprite.flipX = true;
            gunSprite.flipY = true;
        }
        else
        {
            // Mouse is to the right: Face normal
            playerSprite.flipX = false;
            gunSprite.flipY = false;
        }
    }

    private void Shoot()
    {
        // 3. Calculate the exact direction from the player to the mouse
        Vector2 lookDirection = mousePosition - (Vector2)firePoint.position;

        // 4. Calculate the angle in degrees. 
        // We subtract 90f because Unity 2D sprites usually face "Up" on the Y axis by default.
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

        // 5. Spawn the bullet!
        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
    }
}