using UnityEngine;

public class DisappearTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering is the enemy
        if (other.CompareTag("Enemy")) // Make sure your enemy has the tag "Enemy"
        {
            Destroy(other.gameObject); // This will remove the enemy from the scene
        }
    }
}
