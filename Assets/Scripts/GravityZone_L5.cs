using UnityEngine;

public class GravityZone_L5 : MonoBehaviour
{
    public PlayerController_L5.GravityDirection gravityDirection;
    public Color zoneColor = Color.green;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController_L5 pc = other.GetComponent<PlayerController_L5>();
        if (pc != null)
        {
            // Tell player they CAN switch gravity now
            pc.EnableGravitySwitch(gravityDirection);
            Debug.Log($"✅ Entered {gravityDirection} GravityZone - Player can now switch!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController_L5 pc = other.GetComponent<PlayerController_L5>();
        if (pc != null)
        {
            // Disable gravity switching when leaving the zone
            pc.DisableGravitySwitch();
            Debug.Log($"🚪 Left {gravityDirection} GravityZone");
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
            UnityEditor.Handles.Label(transform.position, gravityDirection.ToString());
#endif
        }
    }
}