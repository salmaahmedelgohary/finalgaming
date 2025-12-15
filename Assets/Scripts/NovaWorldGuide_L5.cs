using UnityEngine;
using UnityEngine.UI;

public class NovaWorldGuide_L5 : MonoBehaviour
{
    [Header("Nova Character")]
    public GameObject novaCharacter; // The Nova sprite in the world
    public SpriteRenderer novaSprite; // Nova's sprite renderer

    [Header("Speech Bubble")]
    public GameObject speechBubble; // The speech bubble UI
    public Text speechText; // The text in the bubble

    [Header("Settings")]
    public Vector3 novaOffset = new Vector3(1.5f, 0.5f, 0f); // Offset from player
    public float bobSpeed = 2f; // How fast Nova bobs up and down
    public float bobAmount = 0.2f; // How much Nova bobs
    public bool followPlayer = true; // Should Nova follow the player?

    private Transform playerTransform;
    private Vector3 initialNovaPosition;
    private float bobTime = 0f;
    private bool isActive = false;

    void Start()
    {
        // Find the player
        GameObject player = GameObject.Find("Zero");
        if (player != null)
            playerTransform = player.transform;

        // Hide Nova at start
        HideNova();
    }

    void Update()
    {
        if (!isActive) return;

        // Follow player if enabled
        if (followPlayer && playerTransform != null)
        {
            Vector3 targetPos = playerTransform.position + novaOffset;
            novaCharacter.transform.position = Vector3.Lerp(
                novaCharacter.transform.position,
                targetPos,
                Time.deltaTime * 5f
            );
        }

        // Bobbing animation
        bobTime += Time.deltaTime * bobSpeed;
        float bobOffset = Mathf.Sin(bobTime) * bobAmount;

        Vector3 pos = novaCharacter.transform.position;
        pos.y += bobOffset * Time.deltaTime * 10f;
        novaCharacter.transform.position = pos;

        // Make speech bubble follow Nova
        if (speechBubble != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(novaCharacter.transform.position);
            screenPos.y += 80; // Position bubble above Nova
            speechBubble.transform.position = screenPos;
        }
    }

    public void ShowNovaWithMessage(string message, Vector3 worldPosition, bool shouldFollowPlayer = true)
    {
        isActive = true;
        followPlayer = shouldFollowPlayer;

        // Position Nova
        novaCharacter.transform.position = worldPosition;
        novaCharacter.SetActive(true);

        // Show speech bubble
        if (speechBubble != null && speechText != null)
        {
            speechText.text = message;
            speechBubble.SetActive(true);
        }

        bobTime = 0f;

        Debug.Log($"🤖 Nova appears: {message}");
    }

    public void HideNova()
    {
        isActive = false;

        if (novaCharacter != null)
            novaCharacter.SetActive(false);

        if (speechBubble != null)
            speechBubble.SetActive(false);
    }
}