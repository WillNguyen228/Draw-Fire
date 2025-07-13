using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float stopDistance = 0.1f;

    private GameObject player;
    private Rigidbody2D rb;
    private bool isStopped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found! Make sure it's tagged 'Player'.");
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;

        if (!isStopped && distance < stopDistance)
        {
            isStopped = true;
            // Freeze everything when stopping
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (isStopped && distance > stopDistance + 0.2f) // small buffer to prevent glitch
        {
            isStopped = false;
            // Unfreeze movement, keep rotation locked
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (!isStopped)
        {
            direction.Normalize();
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            rb.MovePosition(rb.position);
        }
    }
}
