using UnityEngine;

public class Goblin : MonoBehaviour
{
    public int maxHealth = 60;
    public int currentHealth;

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
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
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
}
