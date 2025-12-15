using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Legacy{
public class skils : MonoBehaviour
{
    public float cooldownDuration = 5f; // seconds for skill cooldown
    public float stopDuration = 2f;     // how long to stop spike movers
    public KeyCode activateKey = KeyCode.Space; // keyboard trigger

    // Temporal Dash settings
    public KeyCode dashKey = KeyCode.LeftShift; // key to trigger temporal dash
    public float dashDistance = 1f;             // how far to skip on x axis
    public float dashCooldownDuration = 5f;     // cooldown for temporal dash

    bool isOnCooldown = false;
    float cooldownTimer = 0f;
    bool isDashOnCooldown = false;
    float dashCooldownTimer = 0f;

    void Update()
    {
        if (Input.GetKeyDown(activateKey))
        {
            TryActivate();
        }

        if (Input.GetKeyDown(dashKey))
        {
            TryTemporalDash();
        }

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }

        if (isDashOnCooldown)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0f)
            {
                isDashOnCooldown = false;
            }
        }
    }

    // Keyboard-only activation
    public void TryActivate()
    {
        if (isOnCooldown) return;

        SpikeMover.PauseAll(stopDuration);
        SpikeMoverHorizontal.PauseAll(stopDuration);

        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
    }

    // Teleport the character by one "digit" (unit) along the X axis
    // in the direction the character is facing. Has its own 5s cooldown.
    public void TryTemporalDash()
    {
        if (isDashOnCooldown) return;

        // Determine facing direction with multiple fallbacks:
        // 1) SpriteRenderer.flipX if present (common for 2D sprites)
        // 2) transform.localScale.x sign (common flip method)
        // 3) Rigidbody2D velocity.x sign (if character is moving)
        float direction = 1f;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            direction = sr.flipX ? -1f : 1f;
        }
        else if (Mathf.Abs(transform.localScale.x) > 0.0001f)
        {
            direction = transform.localScale.x < 0f ? -1f : 1f;
        }
        else
        {
            Rigidbody2D rb2 = GetComponent<Rigidbody2D>();
            if (rb2 != null && Mathf.Abs(rb2.velocity.x) > 0.01f)
            {
                direction = rb2.velocity.x < 0f ? -1f : 1f;
            }
        }

        Vector3 target = transform.position + new Vector3(direction * dashDistance, 0f, 0f);

        // Perform the dash (instant teleport). Caller can add collision checks if needed.
        transform.position = target;

        isDashOnCooldown = true;
        dashCooldownTimer = dashCooldownDuration;
    }
}
}