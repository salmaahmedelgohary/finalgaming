using UnityEngine;
using UnityEngine.UI;   // If you use normal UI Text
// using TMPro;        // Uncomment if you use TextMeshPro

public class Dialogue3 : MonoBehaviour
{
    public GameObject dialoguePanel;  // The panel with the background
    public Text dialogueText;         // For normal UI Text
    // public TextMeshProUGUI dialogueText; // Use this instead if you use TMP

    [TextArea(2, 5)]
    public string line =
        "ZERO, THIS ROAD IS PURE DANGER — STAY SHARP AND BE READY FOR ANYTHING THAT COMES!";

    void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);  // Hide the panel at the start
        }
    }

    // Call this from your Button OnClick
    public void ShowDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = line;
    }

    // Optional: call this to close the dialogue (another button or after some event)
    public void HideDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}
