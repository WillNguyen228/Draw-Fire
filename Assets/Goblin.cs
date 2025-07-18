using UnityEngine;

public class Goblin : MonoBehaviour
{
    public int maxHealth = 60;
    public int currentHealth;
    public EnermySpawning spawner;  // set by the spawner script
    public Health healthBar;
    public Animator animator; // Assign in Inspector
    private bool isDead = false;

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
    }

    void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage called with damage: " + damage);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDied();
        }

        // Optional: play death animation, sound, etc. here
        isDead = true;
        Debug.Log("Triggering Die animation");
        Debug.Log("Animator exists: " + (animator != null));
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        else
        {
            Debug.LogError("Animator is NULL! Assign it in the Inspector.");
        }
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
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
            TakeDamage(30);
        }
    }
}
