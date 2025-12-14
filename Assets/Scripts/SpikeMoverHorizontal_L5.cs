using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMoverHorizontal_L5 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 2.0f;     // How far left/right it moves
    public float speed = 2.0f;            // Movement speed

    private Vector2 startPos;
    private bool movingRight = true;

    void Start()
    {
        // Save the starting position
        startPos = transform.position;
    }

    void Update()
    {
        MoveSpike();
    }

    void MoveSpike()
    {
        // Choose target position based on direction
        Vector2 target = startPos + (movingRight ? Vector2.right * moveDistance : Vector2.left * moveDistance);

        // Move towards target
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Switch direction when reaching target
        if (Vector2.Distance(transform.position, target) < 0.01f)
        {
            movingRight = !movingRight;
        }
    }
}
