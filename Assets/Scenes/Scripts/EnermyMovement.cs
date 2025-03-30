using UnityEngine;

public class EnermyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;  // Movement speed of the enemy
    public float knockbackForce = 5f;  // Force applied when knocked back
    private Vector3 targetPosition;
    private bool isKnockedBack = false;
    private Vector3 knockbackDirection;

    void Start()
    {
        // Set the target to the center of the screen
        targetPosition = new Vector3(0, 0, 0);  // Target at the center of the screen
    }

    void Update()
    {
        // If knocked back, move in the knockback direction
        if (isKnockedBack)
        {
            Debug.Log("Target got reached! Applying knockback");
            KnockbackMovement();
        }
        else
        {
        // Move the enemy towards the target
        MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        // Calculate the direction to move towards the target
        Vector3 direction = (targetPosition - transform.position).normalized;
        
        // Move the enemy towards the target
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void KnockbackMovement()
    {
        // Move the enemy in the knockback direction
        transform.position += knockbackDirection * knockbackForce * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        // Check if the enemy hits the target at (0, 0, 0)
        if (collision.gameObject.CompareTag("Target"))
        {
            // Calculate the knockback direction
            knockbackDirection = (transform.position - targetPosition).normalized;

            // Trigger knockback
            isKnockedBack = true;

            // Optionally: Add a delay to stop knockback after a short time (e.g., 1 second)
            Invoke("StopKnockback", 1f);
        }
    }

    void StopKnockback()
    {
        // Stop knockback after a short time
        isKnockedBack = false;
    }
}
