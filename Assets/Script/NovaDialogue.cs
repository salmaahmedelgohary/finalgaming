using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            string[] dialogue = {"Zero: Who are you? What is this place?",
            "Nova: I am a stabilizer protocol. You can call me Nova. This... is a broken sanctuary. Your sanctuary.","Zero: I don't remember anything. Just... falling. And then I could change the fall.",
            "Nova: \"This realm's rules are shattered. Your control over gravity is the only logic that remains.","Nova: \"I will help you navigate the collapse. But you must trust me."
            };
            dialogueManager.SetSentences(dialogue);
            StartCoroutine(dialogueManager.TypeDialogue());

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
