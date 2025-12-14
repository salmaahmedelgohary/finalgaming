using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingObject : MonoBehaviour
{
    // Adjust the blink interval in the Inspector
    public float blinkInterval = 3f;

    // New: How much damage this object deals
    public int damageAmount = 10; 

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Start the blinking routine
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // --- 1. Object Disappears ---
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
                // NEW: Disable the collider when invisible so it can't damage
                GetComponent<Collider2D>().enabled = false; 
            }
            yield return new WaitForSeconds(blinkInterval);

            // --- 2. Object Appears ---
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
                // NEW: Enable the collider when visible so it can damage
                GetComponent<Collider2D>().enabled = true; 
            }
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // NEW: Collision detection function
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to an object tagged as "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // Try to get the PlayerHealth script from the colliding object
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Damage the player only if the object is currently visible/active (collider is on)
                if (GetComponent<Collider2D>().enabled == true)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}
