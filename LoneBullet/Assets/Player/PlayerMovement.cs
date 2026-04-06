using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private float inputX, inputY;
    [SerializeField] private Rigidbody2D rb;

    void Update()
    {
        // Just grab the inputs
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        // Apply movement. You can now walk left while aiming right!
        rb.linearVelocity = new Vector2(inputX, inputY).normalized * moveSpeed;
    }
}