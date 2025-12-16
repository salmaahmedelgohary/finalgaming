using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   [Tooltip("How fast the bullet travels.")]
    public float speed = 10f;

    [Tooltip("How much damage this bullet deals.")]
    public int damage = 10;
    
    [Tooltip("How long the bullet lives before being destroyed (prevents clutter).")]
    public float lifetime = 2f; 

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Destroy the bullet after its lifetime has expired
        Destroy(gameObject, lifetime);
    }
    
    // Function called by the PlayerFire script to set the initial velocity
    public void Launch(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet hits an object tagged "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Try to get the EnemyHealth component
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            
            if (enemyHealth != null)
            {
                // Deal damage
                enemyHealth.TakeDamage(damage);
            }
            
            // Destroy the bullet on hit
            Destroy(gameObject);
        }
        // Also destroy the bullet if it hits something that isn't the Player
        else if (!other.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}
