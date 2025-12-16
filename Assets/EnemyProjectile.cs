using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Tooltip("How fast the projectile travels.")]
    public float speed = 10f;

    [Tooltip("How much damage this projectile deals to the player.")]
    public int damageToPlayer = 10;
    
    [Tooltip("How long the projectile lives before being destroyed.")]
    public float lifetime = 2f; 

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Ensure the projectile is destroyed after its lifetime
        Destroy(gameObject, lifetime);
    }
    
    // Function called by the StaticEnemy script to set the initial velocity
    public void Launch(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits the object tagged "Player"
        if (other.CompareTag("Player"))
        {
            // Try to get the PlayerHealth component
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                // Deal damage to the player
                playerHealth.TakeDamage(damageToPlayer);
                Debug.Log("Enemy shot hit Player. Dealing " + damageToPlayer + " damage.");
            }
            
            // Destroy the projectile upon hitting the player
            Destroy(gameObject);
        }
        // Destroy the projectile if it hits something that is NOT the enemy that fired it
        else if (!other.CompareTag("Enemy") && !other.CompareTag("IgnoreProjectile")) 
        {
            // This prevents friendly fire and prevents the bullet from persisting infinitely
            Destroy(gameObject);
        }
    }
}
