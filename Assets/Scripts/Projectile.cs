using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // --- Configuration Variables ---

    [Tooltip("The speed at which the projectile travels.")]
    public float speed = 15f; 

    [Tooltip("The amount of damage this projectile deals.")]
    public int damageAmount = 1; 

    [Tooltip("How long the projectile lives before being destroyed (to prevent clutter).")]
    public float lifetime = 3f; 
    
    // The Rigidbody 2D is useful for physics interactions
    private Rigidbody2D rb; 


    void Awake()
    {
        // Get the Rigidbody2D component attached to this object
        rb = GetComponent<Rigidbody2D>(); 
        
        // Start the self-destruct timer immediately
        Destroy(gameObject, lifetime); 
    }

    // --- Initialization and Movement ---
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move the projectile forward based on its local 'right' direction.
        // We use FixedUpdate for physics-related movement.
        // Velocity is a robust way to ensure constant movement independent of framerate.
        rb.velocity = transform.right * speed;
    }


    // This method is called when the projectile's Trigger Collider overlaps with another collider.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the projectile hit an Enemy
        // NOTE: Replace "Enemy" with the actual Tag you use for your enemies.
        if (other.CompareTag("Enemy"))
        {
            // --- DEAL DAMAGE (Example) ---
            
            // Try to get a component that handles health on the object it hit.
            // Example: A script named 'HealthComponent'
            /*
            HealthComponent health = other.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
            */
            
            // 2. Destroy the projectile immediately after it hits an enemy.
            Destroy(gameObject);
        }
        // 3. Check if the projectile hit a Wall or Ground
        // This handles cases where you want the bullet to stop on impact with terrain.
        else if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        // 4. Ignore the player (prevents the bullet from immediately destroying itself 
        // if it slightly overlaps with the character that shot it)
        else if (other.CompareTag("Player"))
        {
            // Do nothing, just continue past the player
            return;
        }
    }
}
