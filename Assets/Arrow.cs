using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction = Vector2.right;

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        // Debug.Log(transform.eulerAngles);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Add hit detection (enemy, wall, etc)
        Destroy(gameObject); // Destroy on hit
    }
}
