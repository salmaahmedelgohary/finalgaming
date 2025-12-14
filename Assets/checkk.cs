using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkk : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Set player's respawn point to this checkpoint
            PlayerRespawn pr = other.GetComponent<PlayerRespawn>();
            if (pr != null)
            {
                pr.SetCheckpoint(transform.position);
            }
        }
    }
}
