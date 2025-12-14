using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TimedShakeAndDestroy : MonoBehaviour
{
    // Configuration Fields (Visible in the Unity Inspector)
    
    [Header("Timing")]
    [Tooltip("How long the object will shake before becoming inactive.")]
    [SerializeField] private float activeDuration = 3f; 
    
    // NEW FIELD: How long the object stays gone before regenerating
    [Tooltip("The time (in seconds) the object stays inactive/destroyed.")]
    [SerializeField] private float regenerationDelay = 2f; 

    [Header("Shake Settings")]
    [Tooltip("The intensity of the shake. Higher value means bigger movement.")]
    [SerializeField] private float shakeMagnitude = 0.1f; 
    [Tooltip("The speed of the shake. Higher value means faster jitter.")]
    [SerializeField] private float shakeSpeed = 50f; 

    // Private fields for internal state and components
    private Collider2D trapCollider;
    private SpriteRenderer spriteRenderer; // Needed to make it visually disappear
    private bool isRunningSequence = false;
    private Vector3 originalPosition;

    // --- Unity Lifecycle Methods ---

    void Start()
    {
        // Get the Collider and SpriteRenderer components
        trapCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;

        // CRITICAL CHECKS:
        if (trapCollider == null || !trapCollider.isTrigger)
        {
             Debug.LogError("The object needs a 2D Collider component with 'Is Trigger' CHECKED to detect the player and maintain solidity!");
        }
        if (spriteRenderer == null)
        {
             Debug.LogError("The object needs a SpriteRenderer component to visually disappear!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check for the player tag and ensure the sequence hasn't started yet
        if (other.CompareTag("Player") && !isRunningSequence)
        {
            // Lock the trigger to prevent re-triggering while the object is shaking
            isRunningSequence = true; 
            
            // Start the time-sensitive sequence
            StartCoroutine(ShakeAndDestroySequence());
        }
    }

    // --- The Main Coroutine Sequence (Disappearance) ---

    private IEnumerator ShakeAndDestroySequence()
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        // 1. Shaking Loop
        while (elapsedTime < activeDuration)
        {
            elapsedTime = Time.time - startTime;
            
            // --- Shake Logic (same as before) ---
            float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) * 2f - 1f; 
            float offsetY = Mathf.PerlinNoise(0f, Time.time * shakeSpeed) * 2f - 1f; 
            
            Vector3 shakeOffset = new Vector3(offsetX, offsetY, 0f) * shakeMagnitude;

            // Apply the shake, maintaining the original position as the center
            transform.position = originalPosition + shakeOffset;

            yield return null; 
        }

        // --- Disappearance Steps ---

        // 2. Stop shaking and reset position
        transform.position = originalPosition;

        // 3. Deactivate collision and visual appearance
        if (trapCollider != null)
        {
            trapCollider.enabled = false;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Makes the object visually disappear
        }
        
        // 4. Start the regeneration timer
        StartCoroutine(RegenerationRoutine());
    }
    
    // --- NEW: Regeneration Coroutine ---
    private IEnumerator RegenerationRoutine()
    {
        Debug.Log($"Waiting for {regenerationDelay} seconds to regenerate...");
        
        // Wait for the specified delay while the object is invisible
        yield return new WaitForSeconds(regenerationDelay); 
        
        // --- Regeneration Steps ---
        
        // 1. Reactivate collision and visual appearance
        if (trapCollider != null)
        {
            trapCollider.enabled = true;
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true; // Makes the object reappear
        }
        
        // 2. Reset state flag to allow re-triggering
        isRunningSequence = false;
        
        Debug.Log("Object has regenerated and is ready to be triggered again.");
    }
}
