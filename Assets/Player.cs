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
    public Vector2 arrowOffset = new Vector2(1f, 0f); // Offset from player center
    private bool facingRight = true;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        FaceCursor(); // Always face mouse

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }
    }

    void FaceCursor()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseX = mouseWorldPosition.x;

        // Flip if mouse is on opposite side
        if (mouseX < transform.position.x && facingRight)
        {
            Flip(false);
        }
        else if (mouseX > transform.position.x && !facingRight)
        {
            Flip(true);
        }
    }

    void Flip(bool faceRight)
    {
        facingRight = faceRight;

        // Flip player scale
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * (facingRight ? 1 : -1);
        transform.localScale = localScale;

        // Set arrow spawn offset manually for left/right
        float xOffset = faceRight ? 0.1f : 0.15f;
        arrowSpawnPoint.localPosition = new Vector3(xOffset, arrowOffset.y, 0f);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Optional: play death animation, sound, etc. here

        Destroy(gameObject); // Remove the goblin from the scene
    }

    // Called by animation event (or manually for testing)
    public void FireArrow()
    {
        if (arrowPrefab == null || arrowSpawnPoint == null)
        {
            Debug.LogWarning("Arrow prefab or spawn point not set!");
            return;
        }

        // 1. Get mouse position in world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f; // keep it on the same 2D plane

        // 2. Calculate direction from spawn point to mouse position
        Vector2 direction = (mouseWorldPosition - arrowSpawnPoint.position).normalized;

        // 3. Instantiate and pass direction
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.direction = direction;
        }

        // Optional: Rotate the arrow to face the direction it's traveling
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
