using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3f;
    private Transform player;

    void Start()
    {
        // Find the player automatically using their tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Move toward the player's position every frame
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }
    }
}