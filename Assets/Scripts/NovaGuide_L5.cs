using UnityEngine;
using UnityEngine.UI;

public class NovaGuide_L5 : MonoBehaviour
{
    [Header("UI References")]
    public GameObject messagePanel; // The UI panel that shows the message
    public Text messageText; // The text component (or use TextMeshPro)
    public Image novaImage; // Nova's character image!

    [Header("Nova Character")]
    public Sprite novaSprite; // Drag your Nova picture here!
    public bool showNovaOnLeft = true; // Nova on left or right of text?

    [Header("Messages")]
    public string rightWallMessage = "Press [E] to walk on the RIGHT WALL!";
    public string leftWallMessage = "Press [E] to walk on the LEFT WALL!";
    public string ceilingMessage = "Press [E] to walk on the CEILING!";
    public string floorMessage = "Press [E] to return to the FLOOR!";

    [Header("Settings")]
    public float messageDuration = 0f; // 0 = stays until player switches
    public bool animateNova = true; // Bounce animation for Nova?

    private float messageTimer = 0f;
    private bool isShowingMessage = false;
    private float bounceTime = 0f;

    void Start()
    {
        // Hide message at start
        if (messagePanel != null)
            messagePanel.SetActive(false);

        // Set Nova's sprite
        if (novaImage != null && novaSprite != null)
        {
            novaImage.sprite = novaSprite;
        }
    }

    void Update()
    {
        // Auto-hide message after duration (if set)
        if (isShowingMessage && messageDuration > 0)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0)
            {
                HideMessage();
            }
        }

        // Animate Nova with a bounce effect
        if (isShowingMessage && animateNova && novaImage != null)
        {
            bounceTime += Time.deltaTime * 3f;
            float bounce = Mathf.Sin(bounceTime) * 10f; // Bounce up and down
            novaImage.transform.localPosition = new Vector3(
                novaImage.transform.localPosition.x,
                bounce,
                0
            );
        }
    }

    // Called by PlayerController when entering a gravity zone
    public void ShowGravitySwitchMessage(PlayerController_L5.GravityDirection direction, KeyCode key)
    {
        if (messagePanel == null || messageText == null)
        {
            Debug.LogWarning("Nova: UI references not set!");
            return;
        }

        string message = "";

        switch (direction)
        {
            case PlayerController_L5.GravityDirection.Right:
                message = rightWallMessage.Replace("[E]", $"[{key}]");
                break;
            case PlayerController_L5.GravityDirection.Left:
                message = leftWallMessage.Replace("[E]", $"[{key}]");
                break;
            case PlayerController_L5.GravityDirection.Up:
                message = ceilingMessage.Replace("[E]", $"[{key}]");
                break;
            case PlayerController_L5.GravityDirection.Down:
                message = floorMessage.Replace("[E]", $"[{key}]");
                break;
        }

        messageText.text = message;
        messagePanel.SetActive(true);
        isShowingMessage = true;
        messageTimer = messageDuration;
        bounceTime = 0f;

        Debug.Log($"🤖 Nova: {message}");
    }

    // NEW: Show custom message from NovaTriggerZone
    public void ShowCustomGravityMessage(string customMessage, PlayerController_L5.GravityDirection direction, KeyCode key)
    {
        if (messagePanel == null || messageText == null)
        {
            Debug.LogWarning("Nova: UI references not set!");
            return;
        }

        // Replace [E] with actual key in custom message
        string message = customMessage.Replace("[E]", $"[{key}]");

        messageText.text = message;
        messagePanel.SetActive(true);
        isShowingMessage = true;
        messageTimer = messageDuration;
        bounceTime = 0f;

        Debug.Log($"🤖 Nova: {message}");
    }

    // Called when player switches gravity or leaves zone
    public void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);

        isShowingMessage = false;
    }

    // You can call this for other tutorial messages!
    public void ShowCustomMessage(string message, float duration = 3f)
    {
        if (messagePanel == null || messageText == null)
            return;

        messageText.text = message;
        messagePanel.SetActive(true);
        isShowingMessage = true;
        messageTimer = duration;
        bounceTime = 0f;
    }
}