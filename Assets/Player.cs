using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    public Health healthBar;

    [Header("Arrow Shooting")]
    public GameObject arrowPrefab; // Drag your Arrow prefab here in the Inspector
    public Transform arrowSpawnPoint; // Empty GameObject placed at the bow tip

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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

     // Called by animation event (or manually for testing)
    public void FireArrow()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null)
        {
            Debug.LogWarning("Arrow prefab or spawn point not set!");
            return;
        }

        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            bool facingRight = transform.localScale.x > 0;
            arrowScript.direction = facingRight ? Vector2.right : Vector2.left;
        }
    }
}
