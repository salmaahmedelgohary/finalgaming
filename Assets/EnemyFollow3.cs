using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow3 : enemycontroller
{
    [Header("Follow")]
    public Transform player;

    [Header("Ceiling Follow")]
    public float flipDelay = 1f;        // 1 second after player flips
    private float flipTimer = 0f;
    private bool onCeiling = false;     // enemy’s own ceiling state

    private Rigidbody2D rb;
    private PlayerController playerCtrl;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Find player if not set in Inspector
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if (player != null)
            playerCtrl = player.GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Move toward player
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            maxSpeed * Time.fixedDeltaTime
        );

        // Flip horizontally so enemy faces player
        float dx = player.position.x - transform.position.x;
        if (dx > 0 && sr.flipX)         // player on right, enemy facing left
            sr.flipX = false;
        else if (dx < 0 && !sr.flipX)   // player on left, enemy facing right
            sr.flipX = true;
    }

    void Update()
    {
        HandleCeilingFollow();
    }

    void HandleCeilingFollow()
    {
        if (playerCtrl == null) return;

        // If player’s ceiling state is different from enemy’s, start timer
        bool playerOnCeiling = playerCtrl.IsOnCeiling;

        if (playerOnCeiling != onCeiling)
        {
            flipTimer += Time.deltaTime;

            if (flipTimer >= flipDelay)
            {
                if (playerOnCeiling)
                    FlipToCeiling();
                else
                    FlipToFloor();

                flipTimer = 0f;
            }
        }
        else
        {
            flipTimer = 0f;
        }
    }

    void FlipToCeiling()
    {
        onCeiling = true;

        // Invert gravity for enemy
        if (rb != null)
            rb.gravityScale = -Mathf.Abs(rb.gravityScale);

        // Flip vertically (upside down)
        Vector3 scale = transform.localScale;
        scale.y = -Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    void FlipToFloor()
    {
        onCeiling = false;

        // Normal gravity
        if (rb != null)
            rb.gravityScale = Mathf.Abs(rb.gravityScale);

        // Flip back upright
        Vector3 scale = transform.localScale;
        scale.y = Mathf.Abs(scale.y);
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<PlayerStats>().TakeDamage(damage);
        }
        else if (other.CompareTag("Wall"))
        {
            Flip();    // from enemycontroller (horizontal flip)
        }
    }
}
