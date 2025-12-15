using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    // Use this if the enemy collider is NOT a trigger
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Enemy hit player (collision)");
                playerHealth.TakeDamage(damage);
            }
        }
    }

    // If your enemy collider IS a trigger instead, delete the method above
    // and use this one:
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Enemy hit player (trigger)");
                playerHealth.TakeDamage(damage);
            }
        }
    }
    */
}
