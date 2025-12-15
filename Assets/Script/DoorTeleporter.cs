using UnityEngine;

public class DoorTeleporter : MonoBehaviour
{
    public Transform exitPoint;
    public Sprite openSprite;
    public Sprite closedSprite;

    private SpriteRenderer sr;
    private Collider2D doorCollider;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedSprite;

        doorCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Open the door
            sr.sprite = openSprite;

            // Disable this door's collider to prevent immediate re-trigger
            doorCollider.enabled = false;

            // Move player to exit
            collision.transform.position = exitPoint.position;

            PlayerController2 pc = collision.GetComponent<PlayerController2>();
            if (pc != null)
            {
                pc.ResetAfterDoor();
            }

            // Optionally close the door after a short delay
            Invoke(nameof(CloseDoor), 0.3f);

            // Re-enable collider after a short delay
            Invoke(nameof(EnableCollider), 0.3f);
        }
    }

    private void CloseDoor()
    {
        sr.sprite = closedSprite;
    }

    private void EnableCollider()
    {
        doorCollider.enabled = true;
    }
}