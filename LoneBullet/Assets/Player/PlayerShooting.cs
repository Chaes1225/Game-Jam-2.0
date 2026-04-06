using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Setup")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint; // Where the bullet spawns (e.g., the tip of a gun)

    private Camera mainCam;
    private Vector2 mousePosition;

    void Start()
    {
        mainCam = Camera.main; // Automatically find the main camera
    }

    void Update()
    {
        // 1. Find out where the mouse is in the actual game world
        mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // 2. Listen for the left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
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