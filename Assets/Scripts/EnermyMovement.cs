using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed;
    public float stopDistance;
    public float attackRange;         // How close to player before attacking
    public float attackCooldown;      // Time between attacks in seconds
    public int attackDamage;             // Damage done to player per attack
    private GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isStopped = false;
    private float baseScaleX;
    private float lastAttackTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
        animator.SetBool("IsCoolDown", false);

        baseScaleX = transform.localScale.x; // store the original scale.x here (e.g., 3)

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

        // Handle stop/freeze
        if (!isStopped && distance < attackRange) //stopDistance
        {
            isStopped = true;
            // Freeze everything when stopping
            Debug.Log("Freeze everything when stopping!");
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (isStopped && distance > attackRange + 0.2f) // small buffer to prevent glitch
        {
            isStopped = false;
            // Unfreeze movement, keep rotation locked
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        // Movement
        if (!isStopped)
        {
            direction.Normalize();
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            // Animation logic
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", direction.x);

            // Flip sprite for left/right facing (if needed)
            if (Mathf.Abs(direction.x) > 0.1f)
            {
                Vector3 scale = transform.localScale;
                scale.x = baseScaleX * (direction.x > 0 ? 1 : -1);
                transform.localScale = scale;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            rb.MovePosition(rb.position);
            Debug.Log("Second freeze");

            animator.SetBool("IsMoving", false);
        }

        Debug.Log($"[DEBUG] distance: {distance}, attackRange: {attackRange}, time: {Time.time}, next attack at: {lastAttackTime + attackCooldown}");
        // Attack logic — only attack if cooldown elapsed and player is in attack range
        if (distance <= attackRange)
        {
            Debug.Log("[DEBUG] Player is in range.");
        }
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("[DEBUG] Cooldown passed.");
            animator.SetBool("IsCoolDown", false);
        }
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown) //distance <= attackRange && && Time.time >= lastAttackTime + attackCooldown
        {
            Debug.Log("First Attack!");
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        animator?.SetTrigger("Attack");
    }

    // Called from Animation Event at moment of attack hit frame
    public void DealDamage()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(attackDamage);
            }
        }
    }
}
