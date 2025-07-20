using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement & Combat")]
    public float moveSpeed;
    public float stopDistance;
    public float attackRange;         // How close to player before attacking
    public float attackCooldown;      // Time between attacks in seconds
    public int attackDamage;             // Damage done to player per attack
    public GameObject arrowPrefab;
    public Transform RightarrowSpawnPoint;
    public Transform LeftarrowSpawnPoint;
    public bool isArcher = false;

    private bool facingRight = true;
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
    }

    void FixedUpdate()
    {
        if (GameManager.IsGamePaused) return;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player not found! Make sure it's tagged 'Player'.");
        }

        if (player.transform.position.x < transform.position.x)
        {

            FaceLeft();
        }
        else
        {
            FaceRight();
        }
        if (player == null) return;

        Vector2 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;

        // Face the player
        if (player != null)
        {
            if (player.transform.position.x < transform.position.x)
            {
                Debug.Log("Player is facing left");
                facingRight = false;
                FaceLeft();
            }
            else
            {
                Debug.Log("Player is facing right");
                facingRight = true;
                FaceRight();
            }
        }

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
    void FaceLeft()
    {
        if (facingRight)
        {
            facingRight = false;

            Vector3 localScale = transform.localScale;
            localScale.x = -Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }

    void FaceRight()
    {
        if (!facingRight)
        {
            facingRight = true;

            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        if (isArcher)
        {
            animator?.SetTrigger("Attack"); // triggers arrow-shoot animation
        }
        else
        {
            animator?.SetTrigger("Attack"); // still allow melee animation
        }
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

    // This will be called from the animation event during the attack
    public void ShootArrow()
    {
        if (!isArcher || arrowPrefab == null || LeftarrowSpawnPoint == null || RightarrowSpawnPoint == null) return;

        Vector2 direction;
        GameObject arrow;

        // if (LeftarrowSpawnPoint == null) return;
        if (facingRight)
        {
            // 1. Get direction from spawn point to player (not from the archer's root transform!)
            direction = (player.transform.position - RightarrowSpawnPoint.position).normalized;

            // 2. Instantiate arrow at spawn point
            arrow = Instantiate(arrowPrefab, RightarrowSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            // 1. Get direction from spawn point to player (not from the archer's root transform!)
            direction = (player.transform.position - LeftarrowSpawnPoint.position).normalized;

            // 2. Instantiate arrow at spawn point
            arrow = Instantiate(arrowPrefab, LeftarrowSpawnPoint.position, Quaternion.identity);
        }

        // 3. Assign direction to Arrow script (same as player shooting)
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.direction = direction;
        }

        // 4. Rotate to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        Debug.Log("Arrow shot at: " + Time.time);
    }

    public void PerformRadialArrowBurst(Vector2 burstPosition)
    {
        StartCoroutine(PerformRadialArrowBurstWaves(burstPosition));
    }

    private IEnumerator PerformRadialArrowBurstWaves(Vector2 center)
    {
        if (arrowPrefab == null) yield break;

        int numWaves = Random.Range(3, 6); // 3 to 5 waves
        float delayBetweenWaves = 0.4f;
        Debug.Log("Burst center used: " + center);

        for (int wave = 0; wave < numWaves; wave++)
        {
            int numArrows = Random.Range(10, 20);
            float angleOffset = Random.Range(0f, 360f);
            float radius = Random.Range(0.8f, 1.5f);
            float angleStep = 360f / numArrows;

            for (int i = 0; i < numArrows; i++)
            {
                float angle = (i * angleStep + angleOffset + Random.Range(-5f, 5f)) * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
                Vector2 spawnPos = center + direction * radius;

                GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.identity);

                Arrow arrowScript = arrow.GetComponent<Arrow>();
                if (arrowScript != null)
                {
                    arrowScript.direction = direction;
                }

                arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            }

            Debug.Log($"Wave {wave + 1}/{numWaves} fired!");
            yield return new WaitForSeconds(delayBetweenWaves);
        }

        Debug.Log("Radial arrow burst complete!");
    }
}
