using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy
{
    public class CameraFollow : MonoBehaviour
    {
        
       // Public variables for the Inspector
        public Transform Target;
        public float CameraSpeed = 5f; 
        
        public float minX, maxX;
        public float minY, maxY;

        [Header("Rotation Timing")]
        [Tooltip("Time the camera waits at 0 or 180 degrees before starting to rotate.")]
        public float WaitDuration = 50f; 
        
        [Tooltip("Time it takes for the camera to complete the 180-degree rotation (Flip and Flop).")]
        public float RotationDuration = 50f; 
        
        // Private variables for timing and rotation state
        private float timer = 0f;
        private float initialZRotation;
        private float targetZAngle; 
        private float cycleDuration; // Calculated based on public variables

        void Start()
        {
            // Store the initial Z rotation (usually 0 for a 2D camera)
            initialZRotation = transform.eulerAngles.z;
            targetZAngle = initialZRotation; // Start at the initial angle
            
            // Calculate the total cycle duration: Wait(0) + Rotate(180) + Wait(180) + Rotate(0)
            cycleDuration = (WaitDuration * 2) + (RotationDuration * 2);
        }

        void FixedUpdate()
        {
            if (Target != null)
            {
                // --- 1. Position Update (Kept as before) ---
                
                Vector3 targetPosition = Target.position;
                
                Vector3 newCamPosition = Vector3.Lerp(
                    transform.position,
                    new Vector3(targetPosition.x, targetPosition.y, transform.position.z),
                    Time.deltaTime * CameraSpeed
                ); 

                float ClampX = Mathf.Clamp(newCamPosition.x, minX, maxX);
                float ClampY = Mathf.Clamp(newCamPosition.y, minY, maxY);

                transform.position = new Vector3(ClampX, ClampY, transform.position.z);


                // --- 2. Rotation Update (Dynamic Timing) ---
                
                // Recalculate cycle duration in case public values are changed at runtime
                cycleDuration = (WaitDuration * 2) + (RotationDuration * 2);

                // Advance the timer
                timer += Time.deltaTime;

                // Use the modulo operator to repeat the cycle
                float timeInCycle = timer % cycleDuration; 

                // Define the four phases based on dynamic durations:

                if (timeInCycle < WaitDuration)
                {
                    // Phase 1: Wait at 0 degrees
                    targetZAngle = initialZRotation;
                }
                else if (timeInCycle < WaitDuration + RotationDuration)
                {
                    // Phase 2: Rotate 0 -> 180 degrees
                    float t = (timeInCycle - WaitDuration) / RotationDuration; // t goes from 0 to 1
                    targetZAngle = Mathf.Lerp(initialZRotation, initialZRotation + 180f, t);
                }
                else if (timeInCycle < (WaitDuration * 2) + RotationDuration)
                {
                    // Phase 3: Wait at 180 degrees
                    targetZAngle = initialZRotation + 180f;
                }
                else // timeInCycle < cycleDuration
                {
                    // Phase 4: Rotate 180 -> 0 degrees (Return)
                    float timeAtStartOfPhase = (WaitDuration * 2) + RotationDuration;
                    float t = (timeInCycle - timeAtStartOfPhase) / RotationDuration; // t goes from 0 to 1
                    targetZAngle = Mathf.Lerp(initialZRotation + 180f, initialZRotation, t);
                }
                
                // Apply the final calculated rotation
                transform.rotation = Quaternion.Euler(
                    transform.eulerAngles.x, 
                    transform.eulerAngles.y, 
                    targetZAngle 
                );
            }
        }
    }
}
