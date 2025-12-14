
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerEnemyy : enemycontroller
{
    public Transform player;

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, maxSpeed * Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<PlayerStats>().TakeDamage(damage);

        }
        else if (other.tag == "Wall")
        {
            Flip();
        }


    }

    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
