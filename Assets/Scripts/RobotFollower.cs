using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFollower : MonoBehaviour
{

    [Header("Follow Settings")]
    public Transform playerTransform;
    public float followSpeed = 5f;
    public float followSmoothing = 0.15f;
    public Vector2 followOffset;
    
    [Header("Rotation Settings")]
    public float rotationSpeed = 10f;
    public float rotationSmoothing = 0.1f;
    
    [Header("Aiming Settings")]
    private Camera mainCam;
    public Transform crosshair;
    public bool hideSystemCursor = false;
    public float crosshairSmoothing = 0.05f;
    private Vector3 mousePosition;
    private Vector3 smoothedMousePosition;
    private Vector3 initialScale;
    
    [Header("Flip Settings")]
    public float flipSmoothing = 0.1f;

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 10f; 
    public bool automaticFire = true;
    private float fireInterval; 
    private float nextFireTime = 0f;
    
    private Vector3 followVelocity;
    private Vector3 currentRotationVelocity;
    private Vector3 scaleVelocity;
    private Vector3 targetScale;
    private Quaternion targetRotation;
    
    private Animator animator;

    void Start()
    {
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
        
        if (animator != null)
        {
            animator.speed = 1f;
        }
        
        if (hideSystemCursor)
        {
            Cursor.visible = false;
        }
        Cursor.lockState = CursorLockMode.None;

        initialScale = transform.localScale;
        targetScale = initialScale;
        smoothedMousePosition = Input.mousePosition;
        
        fireInterval = 1f / fireRate;
    }

    void Update()
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
        HandleAiming();
        HandleShooting();
    }

    void HandleAiming()
    {
        Vector3 rawMousePos = Input.mousePosition;
        smoothedMousePosition = Vector3.Lerp(smoothedMousePosition, rawMousePos, 
            1f - crosshairSmoothing);
        
        Vector3 screenMousePos = smoothedMousePosition;
        screenMousePos.z = transform.position.z - mainCam.transform.position.z; 
        
        mousePosition = mainCam.ScreenToWorldPoint(screenMousePos);

        if (crosshair != null)
        {
            crosshair.position = Vector3.Lerp(crosshair.position, mousePosition, 
                Time.deltaTime / crosshairSmoothing);
        }

        Vector2 aimDirection = mousePosition - transform.position;
        float targetAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        
        targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 
            rotationSpeed * Time.deltaTime);

        if (Mathf.Abs(targetAngle) > 90)
        {
            targetScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }
        else
        {
            targetScale = initialScale;
        }
        
        transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, 
            ref scaleVelocity, flipSmoothing);
    }

    void HandleShooting()
    {
        if (automaticFire)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireInterval;
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + fireInterval;
                Shoot();
            }
        }
    }

    void LateUpdate()
    {
        Vector3 targetPosition = playerTransform.position;
        targetPosition.x += followOffset.x;
        targetPosition.y += followOffset.y;
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, 
            ref followVelocity, followSmoothing, followSpeed);
    }

    void Shoot()
    {
        // Keeping your custom mouse cursor lock state change and animation trigger
    Cursor.lockState = CursorLockMode.None;
    
    if (animator != null)
    {
        animator.SetTrigger("Shoot");
    }
    
    if (projectilePrefab != null && firePoint != null)
    {
        // 1. Calculate the direction vector from the fire point towards the mouse position.
        // THIS IS THE CRITICAL AIMING LOGIC YOU ALREADY HAD.
        Vector2 shootDirection = (mousePosition - (Vector3)firePoint.position).normalized;
        
        // --- INSTANTIATION ---
        
        // 2. Instantiate the projectile. We will use the calculated shootAngle for rotation.
        float shootAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        Quaternion shootRotation = Quaternion.Euler(0, 0, shootAngle);
        
        // Instantiate the object
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, shootRotation);
        
        // 3. Get the Bullet script
        Bullet bulletScript = projectileGO.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            // 4. Use the Launch method to set the velocity once
            // This is the key change to match the established velocity-based movement logic.
            bulletScript.Launch(shootDirection);
        }
        else
        {
            Debug.LogError("Bullet script is missing or misnamed on the projectilePrefab!");
        }
    }
    }
    
    public void ResetSmoothing()
    {
        followVelocity = Vector3.zero;
        scaleVelocity = Vector3.zero;
        smoothedMousePosition = Input.mousePosition;
        targetScale = initialScale;
        transform.localScale = initialScale;
    }
}