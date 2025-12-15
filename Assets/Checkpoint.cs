using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Legacy{
public class Checkpoint : MonoBehaviour
{
    // A flag to prevent the player from triggering the same checkpoint multiple times
    private bool isActivated = false;

    // We store the PlayerHealth component temporarily during collision
    private PlayerHealth playerHealth; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the object entering the trigger is the Player
        if (other.CompareTag("Player") && !isActivated)
        {
            // 2. Get the PlayerHealth component from the Player
            playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // 3. Update the player's spawn point to the checkpoint's current position
                playerHealth.UpdateSpawnPoint(transform.position);

                // 4. Mark this checkpoint as activated
                isActivated = true;
                
                // Optional: Add visual feedback for the player
                Debug.Log("Checkpoint Reached! Spawn point updated.");
                
                // Optional: You might want to change the checkpoint's sprite color or animation here
                // Example: GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }
}
}