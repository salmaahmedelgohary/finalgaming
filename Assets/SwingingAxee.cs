using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxee : MonoBehaviour
{
    [Header("Swing Settings")]
    [Tooltip("The speed at which the axe swings back and forth.")]
    public float swingSpeed = 2.0f; 
    
    [Tooltip("The maximum angle the axe swings away from its center point.")]
    public float swingLimit = 45f;

    [Header("Damage Settings")]
    [Tooltip("The amount of damage dealt to the player on contact.")]
    public int damageAmount = 25; 

    // Used to store the axe's starting rotation in Z
    private float startRotationZ; 

    void Start()
    {
        // Store the initial rotation of the axe for reference
        startRotationZ = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        // Calculate the current rotation offset using a sine wave
        // Time.time * swingSpeed gives a smooth, continuous value
        // Mathf.Sin() makes it oscillate between -1 and 1
        float angleOffset = Mathf.Sin(Time.time * swingSpeed) * swingLimit;

        // Apply the new rotation around the Z-axis
        // The rotation is centered around the initial startRotationZ
        transform.rotation = Quaternion.Euler(0, 0, startRotationZ + angleOffset);
    }
    
    // --- Damage Logic (Uses the PlayerHealth script) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Try to get the PlayerHealth script
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Deal damage to the player
                playerHealth.TakeDamage(damageAmount);
                
                Debug.Log($"Axe hit Player! Dealing {damageAmount} damage.");
            }
        }
    }
}
