using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCenterOfMass : MonoBehaviour
{
    private Rigidbody2D rb;

    [Tooltip("The local offset from the object's origin to the new Center of Mass.")]
    public Vector2 newCenterOfMass = new Vector2(0f, -0.5f); 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // This line overrides the Composite Collider's calculated CoM.
            rb.centerOfMass = newCenterOfMass;
            
            Debug.Log($"Center of Mass set via script to: {rb.centerOfMass}");
        }
    }
}
