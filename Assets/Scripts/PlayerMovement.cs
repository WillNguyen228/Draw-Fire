using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 4f; // Default speed, editable in Inspector
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.1f;

    private Vector3 movedir;
    private Vector3 ogScale;
    private Collider2D playerCollider;
    private Rigidbody2D playerrgbd;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isDashing = false;
    private float dashTimer = 0f;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        ogScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        playerrgbd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // For flipping
    }

    void Update()
    {
        if (GameManager.IsGamePaused) return;
        float movedirx = 0f;
        float movediry = 0f;

        // Arrow Keys + WASD
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            movediry += 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            movediry -= 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            movedirx -= 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            movedirx += 1f;
        }

        movedir = new Vector3(movedirx, movediry).normalized;

        // Tell Animator whether we're walking
        bool isMoving = movedir.magnitude > 0;
        animator.SetBool("isWalking", isMoving);

        // Flip sprite based on horizontal direction
        if (movedirx != 0)
        {
            // spriteRenderer.flipX = movedirx < 0;
            Vector3 flippedPosition = new Vector3(movedirx, 1.0f, 1.0f);
            transform.localScale = Vector3.Scale(ogScale, flippedPosition);
        }
        
        // Trigger dash on right click (mouse button 1)
        if (Input.GetMouseButtonDown(1) && movedir != Vector3.zero && !isDashing)
        {
            Debug.Log("Dashing!");
            isDashing = true;
            dashTimer = dashDuration;
            playerrgbd.linearVelocity = Vector2.zero; // reset current velocity
            playerrgbd.AddForce(movedir * dashForce, ForceMode2D.Impulse);
        }

        // Manage dash state
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
    }

    void FixedUpdate()
    {
        // if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != -316234913)
        // {
        //     playerrgbd.linearVelocity = movedir * speed;
        // }
        
        if (!isDashing)
        {
            playerrgbd.linearVelocity = movedir * speed;
        }
    }
}
