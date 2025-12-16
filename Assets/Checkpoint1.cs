using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint1 : MonoBehaviour
{
    [Tooltip("If true, this checkpoint can only be used once.")]
    public bool oneTime = true;

    private void Awake()
    {
        // Make sure collider is a trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to the player
        if (!other.CompareTag("Player")) return;

        PlayerStats3 stats = other.GetComponent<PlayerStats3>();
        if (stats != null)
        {
            // Set this position as the new spawn point (convert to 2D)
            stats.UpdateSpawnPoint((Vector2)transform.position);
            Debug.Log("Checkpoint reached at: " + transform.position);
        }

        // Optional: disable after first use
        if (oneTime)
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
