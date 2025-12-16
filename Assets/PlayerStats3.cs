using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats3 : MonoBehaviour
{
    // --- Health and Lives ---
    public int maxHealth = 2;
    private int currentHealth;

    public int maxLives = 2;
    private int currentLives;

    // --- UI ---
    public Image healthBar;          // Drag your UI Image here
    // If your bar is based on 01, fillAmount should be currentHealth / (float)maxHealth

    // --- Invulnerability and Flicker ---
    public float invulnerabilityDuration = 2f;  // Time player is immune after being hit
    public float flickerInterval = 0.1f;        // Speed of the flicker effect
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

        UpdateHealthBar();
    }

    // Public method called by the damaging object (BlinkingObject, enemies, traps, etc.)
    public void Take    (int damageAmount)
    {
        // 1. Check for invulnerability
        if (isInvulnerable)
        {
            Debug.Log("Player hit, but is currently invulnerable.");
            return; // Ignore the damage if immune
        }

        // 2. Apply Damage and Check for Death
        currentHealth -= damageAmount;
        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log($"Player hit! Current Health: {currentHealth}.");
        UpdateHealthBar();

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
            // Game Over
            Debug.Log("Game Over!");
            // Add your full Game Over logic here (e.g., load menu scene)
            gameObject.SetActive(false);
        }
        else
        {
            // Respawn and restore health
            Respawn();
        }
    }

    void Respawn()
    {
        // Move the player back to the spawn point
        transform.position = spawnPoint;

        // Restore full health
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Ensure the player is visible immediately after respawn
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        // Apply temporary invulnerability upon respawn
        StartCoroutine(InvulnerabilityRoutine());
    }

    // --- UI Helper ---
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            // Health bar uses 0–1 value for fillAmount
            healthBar.fillAmount = (float)currentHealth / (float)maxHealth; // e.g. 50/100 = 0.5
        }
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
