using UnityEngine;

public class PhysicsEnemy : MonoBehaviour
{
    public float speed = 200f;
    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate() // Use FixedUpdate for physics
    {
        if (player != null)
        {
            // Calculate direction vector
            Vector2 direction = (player.position - transform.position).normalized;

            // Apply velocity directly
            rb.linearVelocity = direction * speed * Time.fixedDeltaTime;
        }
    }
}