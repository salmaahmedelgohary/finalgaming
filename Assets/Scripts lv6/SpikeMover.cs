using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Legacy{
public class SpikeMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 2.0f;     // How far up/down it moves
    public float speed = 2.0f;            // Movement speed

    private Vector2 startPos;
    private bool movingUp = true;
    private float pauseTimer = 0f;

    void Start()
    {
        // Save the starting position
        startPos = transform.position;
    }

    void Update()
    {
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        MoveSpike();
    }

    void MoveSpike()
    {
        // Choose target position based on direction
        Vector2 target = startPos + (movingUp ? Vector2.up * moveDistance : Vector2.down * moveDistance);

        // Move towards target
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Switch direction when reaching target
        if (Vector2.Distance(transform.position, target) < 0.01f)
        {
            movingUp = !movingUp;
        }
    }

    // Pause this instance for `seconds` seconds
    public void Pause(float seconds)
    {
        pauseTimer = Mathf.Max(pauseTimer, seconds);
    }

    // Pause all SpikeMover instances
    public static void PauseAll(float seconds)
    {
        var all = FindObjectsOfType<SpikeMover>();
        foreach (var s in all)
        {
            if (s != null) s.Pause(seconds);
        }
    }
}
}