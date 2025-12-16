using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [Tooltip("The bullet prefab to be instantiated.")]
    public GameObject bulletPrefab;

    [Tooltip("The point from which the bullet is spawned (e.g., the tip of the gun).")]
    public Transform firePoint;

    [Tooltip("Time delay between shots.")]
    public float fireRate = 0.5f;

    private float nextFireTime;

    void Update()
    {
        // --- MODIFIED LINE ---
        // Check if the Spacebar is pressed down AND the fire rate delay has passed.
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Bullet Prefab or Fire Point is not assigned!");
            return;
        }

        // 1. Instantiate the bullet at the Fire Point's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        
        // 2. Get the Bullet script component
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            // 3. Determine the direction of the shot
            // For a simple 2D game, we usually use the right vector of the player's transform
            Vector2 fireDirection = transform.right; 
            
            // If your player is controlled to flip horizontally, you must ensure 'transform.right' 
            // is always facing the way the player is facing (e.g., if scale.x is negative, fireDirection.x should be negative)
            if (transform.localScale.x < 0) 
            {
                fireDirection = -transform.right;
            }

            // 4. Launch the bullet
            bulletScript.Launch(fireDirection);
        }
    }
}
