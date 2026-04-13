using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 25;
    [SerializeField] private float damageInterval = 1f; // 1 second between hits

    private float nextDamageTime;
    private GameManager gameManager;

    void Start()
    {
        // Automatically find the GameManager in your scene so we can update the health bar
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("Enemy couldn't find the GameManager! Make sure it is in the scene.");
        }
    }

    // 1. Triggered the exact moment they touch
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamage();

            // Set the timer so the next hit doesn't happen until 1 second from now
            nextDamageTime = Time.time + damageInterval;
        }
    }

    // 2. Triggered continuously while they are still touching
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if 1 second has passed since the last time we dealt damage
            if (Time.time >= nextDamageTime)
            {
                DealDamage();

                // Reset the timer for the next hit
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    private void DealDamage()
    {
        if (gameManager != null)
        {
            gameManager.TakeDamage(damageAmount);
        }
    }
}