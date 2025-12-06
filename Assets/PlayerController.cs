using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 10f;

    [Header("Input Keys")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode L;
    public KeyCode R;

    [Header("Ground Check")]
    public Transform controller;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    // State variables
    private bool grounded;
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        // Get components once at start for better performance
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Validate required components
        if (rb == null) Debug.LogError("Rigidbody2D component missing!");
        if (anim == null) Debug.LogError("Animator component missing!");
        if (controller == null) Debug.LogError("Ground Check transform not assigned!");
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateAnimations();
    }

    void HandleInput()
    {
        // Jump input
        if (Input.GetKeyDown(jumpKey) && grounded)
        {
            Jump();
        }

        // Movement input
        float horizontalInput = 0f;

        if (Input.GetKey(L))
        {
            horizontalInput = -1f;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = true;
            }
        }
        else if (Input.GetKey(R))
        {
            horizontalInput = 1f;
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = false;
            }
        }

        // Apply movement velocity
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        isJumping = true;

        // Trigger jump animation
        if (anim != null)
        {
            anim.SetTrigger("Jump");
        }
    }

    void UpdateAnimations()
    {
        if (anim == null) return;

        // Speed for walk/run animation
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // Height for jump/fall animation (optional)
        anim.SetFloat("Height", rb.velocity.y);

        // Grounded state
        anim.SetBool("Grounded", grounded);

        // Reset jumping state when landing
        if (isJumping && grounded && rb.velocity.y <= 0)
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        // Update grounded state with physics
        bool wasGrounded = grounded;
        grounded = Physics2D.OverlapCircle(controller.position, groundCheckRadius, whatIsGround);

        // Trigger landing if just touched ground
        if (!wasGrounded && grounded && isJumping)
        {
            // You can add a landing effect or sound here
            isJumping = false;
        }
    }

    // Visual debug for ground check
    void OnDrawGizmosSelected()
    {
        if (controller != null)
        {
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(controller.position, groundCheckRadius);
        }
    }
}