using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 3f;

    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        // transform.up moves the bullet in the direction it is currently facing
        rb.linearVelocity = transform.up * speed;

        // Destroy the bullet after 'lifetime' seconds to save memory
        Destroy(gameObject, lifetime);
    }

    // This triggers when the bullet hits something with a Collider2D
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Example: if (hitInfo.CompareTag("Enemy")) { // Damage Enemy }

        // Destroy the bullet when it hits anything
        Destroy(gameObject);
    }
}