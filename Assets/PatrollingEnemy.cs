using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("The speed at which the enemy moves.")]
    public float speed = 2f;
    
    [Tooltip("The total distance the enemy will patrol (half on each side of the start point).")]
    public float patrolDistance = 5f;

    [Header("Damage")]
    [Tooltip("The amount of damage this enemy deals when the player touches it.")]
    public int touchDamageAmount = 20;

    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        // Record the position where the enemy starts patrolling.
        startPosition = transform.position;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        // Determine the target position based on the start position and half the patrol distance
        float maxRight = startPosition.x + patrolDistance / 2f;
        float maxLeft = startPosition.x - patrolDistance / 2f;

        if (movingRight)
        {
            // Move right
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            // Check if the enemy has reached the maximum right boundary
            if (transform.position.x >= maxRight)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            // Move left
            transform.Translate(Vector2.left * speed * Time.deltaTime);

            // Check if the enemy has reached the maximum left boundary
            if (transform.position.x <= maxLeft)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        // Simple way to flip the enemy's sprite/model visually
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Reverse the X scale
        transform.localScale = scale;
    }

    // --- Player Collision Logic (Damage on Touch) ---
    // This is the same logic used in your StaticEnemy.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(touchDamageAmount);
                Debug.Log($"Patrolling Enemy hit Player! Dealing {touchDamageAmount} damage.");
            }
        }
    }
}
