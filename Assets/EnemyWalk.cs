using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction = Vector2.left; // change to right/up/down if you want

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
