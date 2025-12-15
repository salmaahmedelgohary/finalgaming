using UnityEngine;

public class checkkkk : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Set player's respawn point to this checkpoint
            other.GetComponent<PlayerController>().SetCheckpoint(transform.position);
        }
    }
}
