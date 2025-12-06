using UnityEngine;
using UnityEngine.UI;

public class NovaClick : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string message = "Gravity can be inverted.";

    private bool clickedOnce = false;

    void OnMouseDown()
    {
        if (!clickedOnce)
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = message;
            clickedOnce = true;
        }
    }
}