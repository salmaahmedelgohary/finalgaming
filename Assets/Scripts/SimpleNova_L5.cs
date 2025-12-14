using UnityEngine;
using UnityEngine.UI;

// Put this script on ONE GameObject and forget everything else!
public class SimpleNova_L5 : MonoBehaviour
{
    [Header("⭐ DRAG YOUR STUFF HERE ⭐")]
    public GameObject novaSprite; // The Nova character sprite in the game
    public GameObject messagePanel; // The UI panel (from Canvas)
    public Text messageText; // The text in the panel

    [Header("⚙️ Settings")]
    public float novaScale = 0.2f; // How big Nova should be (SMALLER!)
    public Vector3 offsetFromPlayer = new Vector3(1.5f, 1f, 0f); // Where Nova appears relative to player
    public float speechBubbleOffset = 0.8f; // How far above Nova the bubble appears

    private bool isShowing = false;
    private Transform player;

    void Start()
    {
        // Find player
        player = GameObject.Find("Zero").transform;

        // Hide everything at start
        if (novaSprite != null)
        {
            novaSprite.SetActive(false);
            novaSprite.transform.localScale = new Vector3(novaScale, novaScale, 1f);
        }

        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    void Update()
    {
        // If showing, make Nova follow player
        if (isShowing && novaSprite != null && player != null)
        {
            novaSprite.transform.position = player.position + offsetFromPlayer;

            // Make speech bubble follow Nova (in screen space)
            if (messagePanel != null && Camera.main != null)
            {
                Vector3 worldPos = novaSprite.transform.position + new Vector3(0, speechBubbleOffset, 0); // Above Nova
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                messagePanel.transform.position = screenPos;
            }
        }
        else
        {
            // IMPORTANT: Hide panel when not showing!
            if (messagePanel != null && !isShowing)
            {
                messagePanel.SetActive(false);
            }
        }
    }

    // Call this from trigger zones!
    public void ShowNova(string message)
    {
        isShowing = true;

        if (novaSprite != null)
        {
            novaSprite.SetActive(true);
            novaSprite.transform.position = player.position + offsetFromPlayer;
        }

        if (messagePanel != null && messageText != null)
        {
            messageText.text = message;
            messagePanel.SetActive(true);
        }

        Debug.Log("🤖 Nova: " + message);
    }

    // Call this when player leaves trigger!
    public void HideNova()
    {
        isShowing = false;

        if (novaSprite != null)
            novaSprite.SetActive(false);

        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}