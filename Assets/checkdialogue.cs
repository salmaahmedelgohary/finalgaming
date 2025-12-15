using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkdialogue : MonoBehaviour
{
    public DialogueManager3 dialogueManagerr;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            string[] dialogue = new string[]
            {
                "Watch out! There is an enemy ahead.",
                "Zero: I think I saw this creature before.",
                "Architect: Ahahahaha! You cannot escape your fate."
            };
            dialogueManagerr.SetSentences(dialogue);
            StartCoroutine(dialogueManagerr.TypeDialogue());

            Destroy(GetComponent<BoxCollider2D>(), 5f);

        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}