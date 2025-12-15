using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // --- Health and Lives ---
    public int maxHealth = 100;
    private int currentHealth;

    public int maxLives = 5;
    private int currentLives;

    // --- Invulnerability and Flicker ---
    public float invulnerabilityDuration = 2f; // Time player is immune after being hit
    public float flickerInterval = 0.1f;       // Speed of the flicker effect
    private bool isInvulnerable = false;

    // --- Components and Respawn ---
    private SpriteRenderer spriteRenderer;
    
    // Set the starting position in the Inspector, or find the actual spawn point/checkpoint
    public Vector3 spawnPoint = Vector3.zero; 

    void Awake()
    {
        // Get the SpriteRenderer component for the flicker effect
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initialize lives and health
        currentLives = maxLives;
        currentHealth = maxHealth;
        
        // Optional: If you want to grab the initial position as the spawn point
        if (spawnPoint == Vector3.zero)
        {
            spawnPoint = transform.position;
        }
    }

    // Public method called by the damaging object (BlinkingObject)
    public void TakeDamage(int damageAmount)
    {
        // 1. Check for invulnerability
        if (isInvulnerable)
        {
            Debug.Log("Player hit, but is currently invulnerable.");
            return; // Ignore the damage if immune
        }

        // 2. Apply Damage and Check for Death
        currentHealth -= damageAmount;
        Debug.Log($"Player hit! Current Health: {currentHealth}.");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // 3. Start invulnerability and flicker effect
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    public void UpdateSpawnPoint(Vector2 newSpawnPoint)
    {
    spawnPoint = newSpawnPoint;
    }

    void Die()
    {
        currentLives--;
        Debug.Log($"Player died! Lives remaining: {currentLives}");

        if (currentLives <= 0)
        {
            // 4. Game Over
            Debug.Log("Game Over!");
            // Add your full Game Over logic here (e.g., load menu scene)
            gameObject.SetActive(false); 
        }
        else
        {
            // 5. Respawn and restore health
            Respawn();
        }
    }

    void Respawn()
    {
        // Move the player back to the spawn point
        transform.position = spawnPoint;
        // Restore full health
        currentHealth = maxHealth;
        
        // Ensure the player is visible immediately after respawn
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        // Apply temporary invulnerability upon respawn
        StartCoroutine(InvulnerabilityRoutine());
    }

    // --- Coroutine for Invulnerability and Flickering ---
    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        
        float startTime = Time.time;

        // Loop until the invulnerability duration is over
        while (Time.time < startTime + invulnerabilityDuration)
        {
            // Toggle the visibility of the sprite
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            // Wait for a short flicker interval
            yield return new WaitForSeconds(flickerInterval);
        }

        // End of Invulnerability: Reset state
        isInvulnerable = false;
        
        // Ensure the player is visible when immunity ends
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        
        Debug.Log("Player invulnerability ended.");
    }
}
