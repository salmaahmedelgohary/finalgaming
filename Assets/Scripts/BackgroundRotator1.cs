using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotator1 : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("The speed of rotation in degrees per second.")]
    public float rotationSpeed = 5f; // Adjust this value for slower or faster rotation
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the rotation amount for this frame
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Apply the rotation around the Z-axis (for 2D rotation)
        // Space.Self rotates relative to the object's current orientation.
        transform.Rotate(0, 0, rotationAmount, Space.Self);
    }
}
