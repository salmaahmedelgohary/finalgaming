using UnityEngine;

// Put this on each trigger zone where you want Nova to appear!
public class SimpleNovaTrigger_L5 : MonoBehaviour
{
    [Header("⭐ WHAT SHOULD NOVA SAY? ⭐")]
    [TextArea(2, 4)]
    public string novaMessage = "Press [E] to switch gravity!";

    [Header("⭐ WHICH DIRECTION? ⭐")]
    public PlayerController_L5.GravityDirection targetGravity = PlayerController_L5.GravityDirection.Right;

    [Header("Settings")]
    public bool showOnlyOnce = true;
    private bool hasShown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasShown && showOnlyOnce) return;

        PlayerController_L5 pc = other.GetComponent<PlayerController_L5>();
        if (pc != null)
        {
            // Enable gravity switch
            pc.EnableGravitySwitch(targetGravity);

            // Show Nova!
            SimpleNova_L5 nova = FindObjectOfType<SimpleNova_L5>();
            if (nova != null)
            {
                // Replace [E] with actual key
                string message = novaMessage.Replace("[E]", pc.switchGravityKey.ToString());
                nova.ShowNova(message);
            }

            hasShown = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController_L5 pc = other.GetComponent<PlayerController_L5>();
        if (pc != null)
        {
            pc.DisableGravitySwitch();

            // Hide Nova
            SimpleNova_L5 nova = FindObjectOfType<SimpleNova_L5>();
            if (nova != null)
            {
                nova.HideNova();
            }
        }
    }

    // Visual helper
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Gizmos.DrawWireCube(transform.position, box.size);
        }
    }
}