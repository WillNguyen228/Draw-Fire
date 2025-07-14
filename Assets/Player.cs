using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    public Health healthBar;

    [Header("Arrow Shooting")]
    public GameObject arrowPrefab; // Drag your Arrow prefab here in the Inspector
    public Transform arrowSpawnPoint; // Empty GameObject placed at the bow tip

    Quaternion arrowRotation;

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

        // Vector2 direction = arrowSpawnPoint.position - Input.mousePosition;
        Debug.Log(arrowSpawnPoint.position);
        Debug.Log(Input.mousePosition);
        arrowRotation.SetFromToRotation(arrowSpawnPoint.position, Input.mousePosition);
        Debug.Log(arrowRotation);
        Debug.Log(arrowSpawnPoint.rotation*arrowRotation);
        Vector2 firePos = arrowSpawnPoint.position;
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, arrowSpawnPoint.rotation*arrowRotation);
        // Vector2 mousePos = Input.mousePosition;
        // Vector2 arrowDir = mousePos - firePos;
        // arrow.transform.Rotate(arrowDir);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            bool facingRight = transform.localScale.x > 0;
            arrowScript.direction = facingRight ? Vector2.right : Vector2.left;
        }
    }
}
