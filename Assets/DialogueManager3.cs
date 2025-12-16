using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueManager3 : MonoBehaviour
{
 
    public TextMeshProUGUI textDisplay;
    private string[] dialogueSentences;
    private int index = 0;
    public float typingspeed;
    public GameObject continueButton;
    public GameObject dialogueBox;
    public Rigidbody2D playerRB;
    private Coroutine typingCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        if (textDisplay != null)
            textDisplay.richText = false;
        if (continueButton != null)
        {
            continueButton.SetActive(false);
            Button btn = continueButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(NextSentence);
            }
            else
            {
                Debug.LogWarning("DialogueManager3: continueButton has no Button component.");
            }
        }
        else
        {
            Debug.LogWarning("DialogueManager3: continueButton is not assigned in the Inspector.");
        }
        
    }

    public void SetSentences(string[] sentences)
    {
        // Stop any current typing and replace sentences
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        this.dialogueSentences = sentences;
        index = 0;
        if (textDisplay != null)
            textDisplay.text = "";
    }

    public IEnumerator TypeDialogue()
    {
        if (dialogueSentences == null || dialogueSentences.Length == 0) yield break;
        if (index < 0 || index >= dialogueSentences.Length) yield break;

        dialogueBox.SetActive(true);
        if (playerRB != null)
            playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        if (textDisplay != null)
            textDisplay.text = "";

        if (continueButton != null)
            continueButton.SetActive(false);

        foreach (char letter in dialogueSentences[index].ToCharArray())
        {
            if (textDisplay != null)
                textDisplay.text += letter;

            yield return new WaitForSeconds(typingspeed);
        }

        if (continueButton != null)
            continueButton.SetActive(true);
        typingCoroutine = null;
    }

    public void NextSentence()
    {
        Debug.Log("Inside NextSentence");
        continueButton.SetActive(false);
        if(index< dialogueSentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }
            typingCoroutine = StartCoroutine(TypeDialogue());
        }
        else
        {
            textDisplay.text = "";
            dialogueBox.SetActive(false);
            this.dialogueSentences = null;
            index = 0;
            playerRB.constraints = RigidbodyConstraints2D.None;
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

        }
    }
    
}

