using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe : MonoBehaviour
{
    [Header("Swing Parameters")]
    // The maximum angle the axe will swing away from the center (e.g., 45 degrees).
    public float maxAngle = 45.0f; 
    
    // How fast the axe swings back and forth.
    public float swingSpeed = 2.0f; 
    
    [Header("Damage")]
    // The amount of damage the player takes on contact.
    public int damageAmount = 1; 

    // The starting angle offset (optional: makes multiple axes swing out of sync).
    public float phaseOffset = 0.0f; 

    void Update()
    {
        // 1. Calculate the current time value using game time, speed, and offset.
        // Mathf.Sin requires the input to be in radians, but we'll deal with degrees later.
        float timeValue = (Time.time + phaseOffset) * swingSpeed;
        
        // 2. The Mathf.Sin function oscillates between -1 and 1.
        // We multiply this by maxAngle to get the target rotation between -maxAngle and +maxAngle.
        float rotationZ = maxAngle * Mathf.Sin(timeValue);

        // 3. Apply the calculated rotation to the axe's Z-axis.
        // We use Euler angles for rotation.
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    // This function handles the damage logic. It is called when the Is Trigger collider is hit.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // IMPORTANT: Check if the colliding object is the player.
        // You'll need to match this tag/component name to your player object.
        if (other.CompareTag("Player"))
        {
            // You would call your Player Health/Damage function here.
            // Example of a function you would implement:
            // other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);

            Debug.Log("Player was hit by the swinging axe! Damage: " + damageAmount);
        }
    }
}
