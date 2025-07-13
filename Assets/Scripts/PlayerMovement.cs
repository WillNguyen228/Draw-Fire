using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 4f; // Default speed, editable in Inspector

    private Vector3 movedir;
    private Collider2D playerCollider;
    private Rigidbody2D playerrgbd;

    void Start() {
        playerCollider = GetComponent<Collider2D>();
        playerrgbd = GetComponent<Rigidbody2D>();
    }

    void Update() {
        float movedirx = 0f;
        float movediry = 0f;

        // Arrow Keys + WASD
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            movediry += 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            movediry -= 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            movedirx -= 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            movedirx += 1f;
        }

        movedir = new Vector3(movedirx, movediry).normalized;
    }

    void FixedUpdate() {
        playerrgbd.linearVelocity = movedir * speed;
    }
}
