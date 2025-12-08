using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 10f;
  
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode L;
    public KeyCode R;
    public KeyCode flipGravityKey = KeyCode.F;   // NEW

    // Ground check
    public Transform controllerNormal;   // NEW
    public Transform controllerInverted; // NEW
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private Transform currentController; // NEW

    // Components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    // State variables
    private bool grounded;
    private bool isJumping;
    private bool gravityFlipped = false;  // NEW

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (controllerNormal == null) Debug.LogError("Assign normal ground check!");
        if (controllerInverted == null) Debug.LogError("Assign inverted ground check!");

        currentController = controllerNormal;
    }

    void Update()
    {
        HandleInput();
        UpdateAnimations();
    }

    void HandleInput()
    {
        // Gravity Flip
        if (Input.GetKeyDown(flipGravityKey))
        {
            FlipGravity();
        }

        // Jump
        if (Input.GetKeyDown(jumpKey) && grounded)
        {
            Jump();
        }

        // Movement
        float horizontalInput = 0f;

        if (Input.GetKey(L))
        {
            horizontalInput = -1f;
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(R))
        {
            horizontalInput = 1f;
            spriteRenderer.flipX = false;
        }

        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        // Jump direction depends on gravity
        float jumpDir = gravityFlipped ? -1f : 1f;

        rb.velocity = new Vector2(rb.velocity.x, jumpHeight * jumpDir);
        isJumping = true;

        if (anim != null)
        {
            anim.SetTrigger("Jump");
        }
    }

    // NEW — flip gravity
    void FlipGravity()
    {
        gravityFlipped = !gravityFlipped;

        // Reverse physics gravity
        rb.gravityScale *= -1f;

        // Rotate player visually
        transform.localRotation = Quaternion.Euler(0, 0, gravityFlipped ? 180 : 0);

        // Switch ground check point
        currentController = gravityFlipped ? controllerInverted : controllerNormal;
    }

    void UpdateAnimations()
    {
        if (anim == null) return;

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Height", rb.velocity.y);
        anim.SetBool("Grounded", grounded);

        if (isJumping && grounded && rb.velocity.y == 0)
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = Physics2D.OverlapCircle(currentController.position, groundCheckRadius, whatIsGround);

        if (!wasGrounded && grounded)
        {
            isJumping = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (controllerNormal != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(controllerNormal.position, groundCheckRadius);
        }
        if (controllerInverted != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controllerInverted.position, groundCheckRadius);
        }
    }
}