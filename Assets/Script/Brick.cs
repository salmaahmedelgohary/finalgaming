using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.name.Equals("Zero"))
        {
            Invoke("DropPlatform", 0.3f);
            Destroy(gameObject, 2f);
        }
    }

    void DropPlatform()
    {
        rb.isKinematic = false;
    }
    // Update is called once per frame
    //void Update()
   // {
        
   // }
}
