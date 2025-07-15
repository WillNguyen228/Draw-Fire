using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public Vector2 direction;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // If it's no longer visible by any camera, destroy it
        if (rend != null && !rend.isVisible)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Add hit detection (enemy, wall, etc)
        Destroy(gameObject); // Destroy on hit
    }
}
