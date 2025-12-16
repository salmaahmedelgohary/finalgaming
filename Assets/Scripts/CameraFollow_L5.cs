
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_L5 : MonoBehaviour
{

    public Transform Target;
    public float Cameraspeed;

    [Header("Target")]
    public Transform player; // Drag Zero here!

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0, 0, -10); // Camera offset from player
    public float smoothSpeed = 0.125f; // How smooth the camera follows (lower = smoother)
    public bool followX = true; // Follow player horizontally?
    public bool followY = true; // Follow player vertically?
 71f79b3 (Add all Level 5 modifications)

    [Header("Camera Bounds (Optional)")]
    public bool useBounds = false;
    public float minX = -50f;
    public float maxX = 50f;
    public float minY = -50f;
    public float maxY = 50f;

    void LateUpdate()
    {
        if (player == null)
        {
            // Try to find player if not set
            GameObject playerObj = GameObject.Find("Zero");
            if (playerObj != null)
                player = playerObj.transform;
            else
                return; // No player found
        }

        // Calculate desired position
        Vector3 desiredPosition = player.position + offset;

        // Apply follow settings
        if (!followX)
            desiredPosition.x = transform.position.x;
        if (!followY)
            desiredPosition.y = transform.position.y;

        // Smooth follow
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply bounds if enabled
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
        }

        // Always keep the Z position (camera distance)
        smoothedPosition.z = offset.z;

        transform.position = smoothedPosition;
    }
}

