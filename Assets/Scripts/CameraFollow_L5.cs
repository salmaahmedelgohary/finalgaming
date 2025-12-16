
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_L5 : MonoBehaviour
{
// Target to follow
    public Transform Target;
    public float Cameraspeed;

    public float minX, maxX;
    public float minY, maxY;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Target != null)
        {
            Vector3 targetPos = new Vector3(Target.position.x, Target.position.y, -10f);
            Vector3 newPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * Cameraspeed);
            transform.position = new Vector3(
                Mathf.Clamp(newPos.x, minX, maxX),
                Mathf.Clamp(newPos.y, minY, maxY),
                -10f
            );
        }
    }

}

