using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    [Header("Damage & Health")]
    [Tooltip("The amount of damage this enemy deals when the player touches it.")]
    public int touchDamageAmount = 20;
    
    // Note: The EnemyHealth script handles maxHealth and destruction.

    [Header("Firing Settings")]
    [Tooltip("The bullet prefab the enemy will fire.")]
    public GameObject bulletPrefab;

    [Tooltip("The point from which the bullet is spawned (e.g., the gun muzzle).")]
    public Transform firePoint;

    [Tooltip("Time delay between shots.")]
    public float fireRate = 2.0f; // Set to 2.0 seconds as requested

    private float nextFireTime;

    void Start()
    {
        // Initialize the first shot to happen soon after the game starts
        nextFireTime = Time.time + fireRate; 

        // CRITICAL CHECK: Ensure all components are linked
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Enemy firing setup is incomplete! Bullet Prefab or Fire Point is missing on " + gameObject.name);
        }
    }

    void Update()
    {
        // Check if enough time has passed to fire another shot
        if (Time.time > nextFireTime)
        {
            Shoot();
            // Set the time for the next shot
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    EnemyProjectile enemyProjectileScript = bullet.GetComponent<EnemyProjectile>();

    if (enemyProjectileScript != null)
    {
        // 1. Get the direction from the Fire Point
        Vector2 fireDirection = Vector2.left; 
        
        // 2. Call the Launch function
        enemyProjectileScript.Launch(fireDirection);
    }
    else
    {
        // Add this error message to confirm the script is on the prefab
        Debug.LogError("EnemyProjectile script is missing on the instantiated bullet!");
    }
    }

    // --- Player Collision Logic (from previous version) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(touchDamageAmount);
                Debug.Log($"Static Enemy hit Player! Dealing {touchDamageAmount} damage.");
            }
        }
    }
}
