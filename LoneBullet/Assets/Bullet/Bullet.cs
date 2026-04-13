using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private Rigidbody2D rb;

    public Rigidbody2D Rigidbody
    {
        get
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            return rb;
        }
    }

    public void Launch(Vector2 direction)
    {
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        Rigidbody.linearVelocity = direction.normalized * speed;
    }

    public void Stop()
    {
        Rigidbody.linearVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            // Find the GameManager in the scene
            GameManager gm = FindFirstObjectByType<GameManager>();

            // If we found it, add 10 points! (You can change this number)
            if (gm != null)
            {
                gm.AddScore(10);
            }

            Destroy(other.gameObject);
        }
    }

}
