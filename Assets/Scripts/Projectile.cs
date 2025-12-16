using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // --- Configuration Variables ---

    [Tooltip("How fast the projectile travels.")]
    public float speed = 15f; 

    [Tooltip("The amount of damage this projectile deals.")]
    public int damage = 10; // Renamed for consistency with PlayerFire script

    [Tooltip("How long the projectile lives before being destroyed (to prevent clutter).")]
    public float lifetime = 3f; 
    
    private Rigidbody2D rb; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        
        // Start the self-destruct timer immediately
        Destroy(gameObject, lifetime); 
    }

    // REMOVED: void Start() {}
    
    // REMOVED: void FixedUpdate() {}

    // --- Initialization and Movement (Launch Method) ---
    // This public method is called once by the PlayerFire script when the bullet is instantiated.
    public void Launch(Vector2 direction)
    {
        if (rb != null)
        {
            // Set the velocity once based on the direction passed in by the PlayerFire script
            rb.velocity = direction.normalized * speed;
        }
    }


    // This method is called when the projectile's Trigger Collider overlaps with another collider.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the projectile hit an Enemy
        if (other.CompareTag("Enemy"))
        {
            // Try to get the EnemyHealth script
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            
            if (enemyHealth != null)
            {
                // Deal damage using the EnemyHealth script
                enemyHealth.TakeDamage(damage); 
            }
            
            // Destroy the projectile immediately after it hits an enemy.
            Destroy(gameObject);
        }
        
        // 2. Check if the projectile hit anything we want it to stop on (e.g., environment)
        // Add any other environment tags you use here (e.g., "Wall", "Ground", "Platform")
        else if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        
        // 3. Ignore the player (prevents the bullet from immediately destroying itself)
        else if (other.CompareTag("Player"))
        {
            // Do nothing, just continue past the player
            return;
        }
    }
}
