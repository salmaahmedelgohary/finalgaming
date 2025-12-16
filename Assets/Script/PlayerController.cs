using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 10f;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode L;
    public KeyCode R;
    public bool IsOnCeiling => onCeiling;

    public KeyCode flipKey = KeyCode.E;   // NEW: flip gravity
    public Transform controller;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private bool grounded;
    private bool isJumping;
    private bool onCeiling = false;       // NEW

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (rb == null) Debug.LogError("Rigidbody2D component missing!");
        if (anim == null) Debug.LogError("Animator component missing!");
        if (controller == null) Debug.LogError("Ground Check transform not assigned!");
    }

    void Update()
    {
        HandleInput();
        UpdateAnimations();
    }

    void HandleInput()
    {
        // Flip between floor and ceiling when on a surface
        if (Input.GetKeyDown(flipKey) && grounded)
        {
            ToggleCeilingWalk();
        }

        // Jump: up when on floor, down when on ceiling
        if (Input.GetKeyDown(jumpKey) && grounded)
        {
            Jump();
        }

        float horizontalInput = 0f;

        if (Input.GetKey(L))
        {
            horizontalInput = -1f;
            if (spriteRenderer != null)
                spriteRenderer.flipX = !onCeiling ? true : false;  // keep facing logic simple
        }
        else if (Input.GetKey(R))
        {
            horizontalInput = 1f;
            if (spriteRenderer != null)
                spriteRenderer.flipX = !onCeiling ? false : true;
        }

        // Move left/right; horizontal direction is the same on floor & ceiling
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void ToggleCeilingWalk()
    {
    onCeiling = !onCeiling;

    rb.gravityScale = -rb.gravityScale;

    Vector3 scale = transform.localScale;
    scale.y *= -1f;
    transform.localScale = scale;

    // Trigger flip animation
    if (anim != null)
    {
        anim.SetTrigger("Flip");
    }
    }

    void Jump()
    {
        // Jump direction depends on gravity
        float dir = Mathf.Sign(rb.gravityScale);   // +1 floor, -1 ceiling
        rb.velocity = new Vector2(rb.velocity.x, jumpHeight * dir);

        isJumping = true;

        if (anim != null)
        {
            anim.SetTrigger("Jump");
        }
    }

    void UpdateAnimations()
    {
        if (anim == null) return;

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Height", rb.velocity.y);
        anim.SetBool("Grounded", grounded);
    }

    void FixedUpdate()
    {
        // Ground check works for both floor and ceiling,
        // because gravity is inverted and the collider touches the surface.
        grounded = Physics2D.OverlapCircle(controller.position, groundCheckRadius, whatIsGround);

        if (isJumping && grounded && ((rb.gravityScale > 0 && rb.velocity.y <= 0) ||
                                      (rb.gravityScale < 0 && rb.velocity.y >= 0)))
        {
            isJumping = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (controller != null)
        {
            Gizmos.color = grounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(controller.position, groundCheckRadius);
        }
    }
}
