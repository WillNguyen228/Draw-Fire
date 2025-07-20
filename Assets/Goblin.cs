using UnityEngine;
using System.Collections;
using DialogueEditor;
using UnityEngine.SceneManagement;

public class Goblin : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 60;
    public int currentHealth;
    public EnermySpawning spawner;  // set by the spawner script
    public Health healthBar;
    public Animator animator; // Assign in Inspector
    private bool isDead = false;

    [Header("Teleport Logic")]
    public Transform[] teleportPoints;
    private int currentTeleportIndex = -1;
    private int damageTaken = 0;
    private int specialAttackDamageTaken = 0;
    public Transform centerTeleportPoint;
    public EnemyMovement enemyMovementScript;
    private bool doSpecialAttack = false;
    private float teleportDelay = 0.5f;
    private float teleportTimer = 0f;

    [Header("Optional Dialogue")]
    public NPCConversation freedKnightDialogue;
    private bool hasTriggeredDialogue = false; // prevent double trigger

    [Header("Is This the Final Boss?")]
    public bool isFinalBoss = false;
    public WinMenu winMenu;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
        // Debug testing triggers
        if (Input.GetKeyDown(KeyCode.T))
        {
            TeleportToRandomCorner();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            TeleportToCenter();
        }
    }

    void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage called with damage: " + damage);
        currentHealth -= damage;
        damageTaken += damage; //For the wizard
        specialAttackDamageTaken += damage; // for special wizard burst

        healthBar.SetHealth(currentHealth);
        Debug.Log("Current health: " + currentHealth);

        // 100 DAMAGE → RANDOM TELEPORT
        if (damageTaken >= 100 && !isDead)
        {
            Debug.Log("Triggering teleport due to 100+ damage");
            TeleportToRandomCorner();
            damageTaken = 0; // RESET COUNTER AFTER TELEPORT
        }

        // 150 DAMAGE → CENTER TELEPORT + SPECIAL ARROW BURST
        if (specialAttackDamageTaken >= 150 && !isDead)
        {
            Debug.Log("Triggering SPECIAL ATTACK due to 150+ damage");

            animator?.SetTrigger("Teleport");
            TeleportToCenter();
            specialAttackDamageTaken = 0;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (spawner != null && !isFinalBoss)
        {
            spawner.OnEnemyDied();
        }

        // Optional: play death animation, sound, etc. here
        isDead = true;
        // Debug.Log("Triggering Die animation");
        Debug.Log("Animator exists: " + (animator != null));
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        // else
        // {
        //     Debug.LogError("Animator is NULL! Assign it in the Inspector.");
        // }

        // If this is the CAGE being broken, start dialogue
        if (isFinalBoss)
        {
            if (freedKnightDialogue != null && !hasTriggeredDialogue)
            {
                hasTriggeredDialogue = true;

                // Time.timeScale = 0f; // ⏸ pause fight
                GameManager.IsGamePaused = true;
                Debug.Log("Pausing the game?: " + GameManager.IsGamePaused);
                Debug.Log("Triggering dialogue: " + freedKnightDialogue);
                // ConversationManager.OnConversationEnded += ResumeAfterDialogue;
                ConversationManager.Instance.StartConversation(freedKnightDialogue);
            }
            else
            {
                // No dialogue? Just show win menu
                ShowVictory();
            }
        }
        else
        {
            // fallback: just destroy if not a special goblin
            Destroy(gameObject, 0.5f);
        }
    }
    // Resume game when dialogue is done
    public void ResumeAfterDialogue()
    {
        // Time.timeScale = 1f;
        GameManager.IsGamePaused = false;
        Debug.Log("Pausing the game?: " + GameManager.IsGamePaused);
        Destroy(gameObject); // Optional: remove cage after dialogue
        // ConversationManager.OnConversationEnded -= ResumeAfterDialogue;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void OnFinalBossDialogueComplete()
    {
        GameManager.IsGamePaused = false;

        Debug.Log("Final boss dialogue complete. Showing win menu.");
        winMenu.Setup();

        Destroy(gameObject); // Remove wizard after dialogue (optional)
    }
    void ShowVictory()
    {
        if (winMenu != null)
        {
            winMenu.Setup();
        }
        else
        {
            Debug.LogWarning("WinMenu not assigned to final boss!");
        }

        // Optionally unlock levels here too
        // You can copy the logic from your previous UnlockNewLevel() method
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something entered Enemy trigger: " + other.name);  // Always prints

        if (other.name == "SwordCollider")
        {
            Debug.Log("Goblin hit by sword!");
            TakeDamage(20);
        }
        if (other.name == "ExplosionCollider")
        {
            Debug.Log("Goblin hit by explosion!");
            TakeDamage(40);
        }
        else if (other.name == "Arrow(Clone)")
        {
            Debug.Log("Enemy hit by arrow!");
            TakeDamage(30);
        }
        if (other.name == "Evil Arrow(Clone)")
        {
            Debug.Log("Enemy hit by arrow!");
            TakeDamage(15);
        }
        if (other.name == "Wizard Blast(Clone)")
        {
            Debug.Log("Enemy hit by blast!");
            TakeDamage(50);
        }
    }

    // ===================== TELEPORT & METEOR LOGIC ======================
    void TeleportToRandomCorner()
    {
        if (teleportPoints.Length == 0) return;

        int nextIndex = Random.Range(0, teleportPoints.Length);
        // If same index as last time, just pick the next one cyclically
        if (teleportPoints.Length > 1 && nextIndex == currentTeleportIndex)
        {
            nextIndex = (nextIndex + 1) % teleportPoints.Length;
        }

        currentTeleportIndex = nextIndex;
        Transform newPos = teleportPoints[currentTeleportIndex];
        Debug.Log("Teleporting to point: " + newPos.name);

        animator?.SetTrigger("Teleport");
        StartCoroutine(TeleportAfterDelay(newPos.position, 0.5f));
    }
    public void TeleportToCenter()
    {
        if (centerTeleportPoint == null) return;

        animator?.SetTrigger("Teleport");
        // StartCoroutine(TeleportAfterDelay(centerTeleportPoint.position, 0.5f));
        StartCoroutine(PerformSpecialAttack(centerTeleportPoint.position, 0.5f));
    }

    IEnumerator TeleportAfterDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = position;
    }
    IEnumerator PerformSpecialAttack(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay); // wait for teleport delay
        transform.position = position; // perform teleport
        yield return null; // wait one frame before bursting

        if (enemyMovementScript != null)
        {
            Debug.Log("Activating the PerformRadialArrowBurst()");
            enemyMovementScript.PerformRadialArrowBurst(position);
        }
        else
        {
            Debug.LogWarning("EnemyMovementScript not assigned!");
        }
    }

}
