using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMover : MonoBehaviour
{
     [Header("Movement Settings")]
    public float moveDistance = 2.0f;     // How far up/down it moves
    public float speed = 2.0f;            // Movement speed

    private Vector3 startPos;
    private bool movingUp = true;
    // Start is called before the first frame update
    void Start()
    {
        // Save the starting position
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveSpike();
    }

    void MoveSpike()
    {
        // Choose target position based on direction
        Vector3 target = startPos + (movingUp ? Vector3.up * moveDistance : Vector3.down * moveDistance);

        // Move towards target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Switch direction when reaching target
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            movingUp = !movingUp;
        }
    }
}
