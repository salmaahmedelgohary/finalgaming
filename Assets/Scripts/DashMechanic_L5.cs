using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this to your existing PlayerController or as a separate component
public class DashMechanic_L5 : MonoBehaviour
{
    [Header("Dash Settings")]
    public KeyCode dashKey = KeyCode.LeftShift;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public int maxDashes = 2; // Double dash!

    [Header("Visual Effects")]
    public TrailRenderer dashTrail; // Optional: add trail effect
    public ParticleSystem dashParticles; // Optional: dash particles

    private Rigidbody2D rb;
    private PlayerController_L5 pc;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private int dashesRemaining;
    private bool isDashing = false;
    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController_L5>();
        dashesRemaining = maxDashes;
    }

    void Update()
    {
        // Cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Reset dashes when grounded
        if (pc != null && pc.IsGrounded())
        {
            dashesRemaining = maxDashes;
        }

        // Dash input
        if (Input.GetKeyDown(dashKey) && dashesRemaining > 0 && cooldownTimer <= 0 && !isDashing)
        {
            StartDash();
        }

        // Handle dash duration
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                EndDash();
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            // Override velocity during dash
            rb.velocity = dashDirection * dashSpeed;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashesRemaining--;
        cooldownTimer = dashCooldown;

        // Get dash direction based on input and gravity orientation
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // If no input, dash in facing direction
        if (inputX == 0 && inputY == 0)
        {
            dashDirection = transform.right; // Dash in facing direction
        }
        else
        {
            // Dash in input direction relative to player orientation
            dashDirection = (transform.right * inputX + transform.up * inputY).normalized;
        }

        // Visual effects
        if (dashTrail != null)
            dashTrail.emitting = true;

        if (dashParticles != null)
            dashParticles.Play();

        Debug.Log($"⚡ DASH! {dashesRemaining} dashes remaining");
    }

    void EndDash()
    {
        isDashing = false;

        // Visual effects
        if (dashTrail != null)
            dashTrail.emitting = false;

        Debug.Log("Dash ended");
    }

    // Public method to check if can dash (for UI feedback)
    public bool CanDash()
    {
        return dashesRemaining > 0 && cooldownTimer <= 0 && !isDashing;
    }

    public int GetDashesRemaining()
    {
        return dashesRemaining;
    }
}