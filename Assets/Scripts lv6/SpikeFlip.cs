using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeFlip : MonoBehaviour
{
       public float flipTime = 2f; // Time between flips (seconds)

    private bool lookingUp = false;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= flipTime)
        {
            FlipSpike();
            timer = 0f;
        }
    }

    void FlipSpike()
    {
        lookingUp = !lookingUp;

        if (lookingUp)
            transform.rotation = Quaternion.Euler(0, 0, 180); // Face up
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);   // Face down
    }
}
