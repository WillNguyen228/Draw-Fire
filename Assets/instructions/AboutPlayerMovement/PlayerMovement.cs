using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // Start is called before the first frame update
    float speed;
    Vector3 movedir;
    Collider2D playerCollider;
    Rigidbody2D playerrgbd;
    void Start() {
        speed = 10f;
        GameObject player = GameObject.Find("Player");
        playerCollider = GetComponent<Collider2D>();
        playerrgbd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        float movedirx = 0f;
        float movediry = 0f;

        if (Input.GetKey(KeyCode.DownArrow)) {
            movediry -= 1f;
        }
        
        if (Input.GetKey(KeyCode.UpArrow)) {
            movediry += 1f;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            movedirx -= 1f;
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            movedirx += 1f;
        }

        movedir = new Vector3(movedirx, movediry).normalized;
    }

    void FixedUpdate() {
        playerrgbd.linearVelocity = movedir * speed;
    }
}
