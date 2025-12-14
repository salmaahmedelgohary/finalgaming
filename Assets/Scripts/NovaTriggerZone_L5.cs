using UnityEngine;

public class NovaTriggerZone_L5 : MonoBehaviour
{
    [Header("What Should Nova Say Here?")]
    [TextArea(3, 5)]
    public string novaMessage = "Press [E] to switch gravity!";

    [Header("Which Gravity Direction?")]
    public PlayerController_L5.GravityDirection targetGravityDirection = PlayerController_L5.GravityDirection.Right;

    [Header("Settings")]
    public bool disableAfterFirstUse = true; // Only show Nova once?
    public Color zoneColor = Color.yellow; // Color in editor

    private bool hasBeenUsed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if already used
        if (hasBeenUsed && disableAfterFirstUse)
            return;

        PlayerController_L5 pc = other.GetComponent<PlayerController_L5>();
        if (pc != null)
        {
            // Enable gravity switching
            pc.EnableGravitySwitch(targetGravityDirection);

            // Show Nova with custom message!
            NovaGuide_L5 nova = FindObjectOfType<NovaGuide_L5>();
            if (nova != null)
            {
                nova.ShowCustomGravityMessage(novaMessage, targetGravityDirection, pc.switchGravityKey);
            }

            Debug.Log($"🤖 Nova Trigger: {novaMessage}");
            hasBeenUsed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController_L5 pc = other.GetComponent<PlayerController_L5>();
        if (pc != null)
        {
            pc.DisableGravitySwitch();

            NovaGuide_L5 nova = FindObjectOfType<NovaGuide_L5>();
            if (nova != null)
            {
                nova.HideMessage();
            }
        }
    }

    // Visual helper in editor
    private void OnDrawGizmos()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Gizmos.color = new Color(zoneColor.r, zoneColor.g, zoneColor.b, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(box.offset, box.size);

#if UNITY_EDITOR
            // Draw Nova's message in editor
            UnityEditor.Handles.Label(transform.position, "NOVA ZONE\n" + novaMessage);
#endif
        }
    }

    // Call this if you want to reset the trigger
    public void Reset()
    {
        hasBeenUsed = false;
    }
}