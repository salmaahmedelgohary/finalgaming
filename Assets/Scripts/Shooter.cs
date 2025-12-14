using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    [Header("Shooting Setup")]
    [Tooltip("The projectile Prefab to be instantiated (e.g., your bullet).")]
    public GameObject projectilePrefab;

    [Tooltip("The point from which the projectile will be shot (e.g., the gun tip).")]
    public Transform firePoint;

    [Tooltip("The speed at which the projectile will travel.")]
    public float projectileSpeed = 10f;

    [Tooltip("The minimum time interval between shots.")]
    public float fireRate = 0.5f;

    private float nextFireTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the current time is greater than the next allowed fire time
        if (Time.time > nextFireTime)
        {
            // Check for the player's fire input (e.g., Left Mouse Button or Ctrl)
            if (Input.GetButtonDown("Fire1")) // "Fire1" is the default input for the primary attack
            {
                Shoot();
                // Set the next time the player can shoot
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("Projectile Prefab or Fire Point is not assigned in the Shooter script!");
            return;
        }

        // 1. Instantiate the Projectile
        // Create a copy of the projectilePrefab at the firePoint's position and rotation.
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 2. Get the Rigidbody2D component of the new projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // 3. Apply Velocity to the Projectile
            // Use the forward direction of the firePoint (which should be facing the direction you want to shoot)
            // Note: firePoint.right is usually used for 2D forward direction.
            rb.velocity = firePoint.right * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Projectile Prefab is missing a Rigidbody2D component!");
        }
        
        // You may want to add code here to destroy the projectile after a few seconds or a certain distance
        // Example: Destroy(projectile, 3f);
    }

}
