using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
   // The amount of damage to deal. Since we want to take one life,
    // we'll set this to a very high number, ensuring the health drops below zero.
    public int instantKillDamage = 9999; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the object entering the trigger is the Player
        if (other.CompareTag("Player"))
        {
            // 2. Try to get the PlayerHealth component from the colliding object
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // 3. Call the TakeDamage function with the instant kill value
                // This will trigger the Die() method in the PlayerHealth script.
                playerHealth.TakeDamage(instantKillDamage);
                
                Debug.Log("Player entered the Death Zone (Lake) and lost a life!");
            }
        }
    }
}
