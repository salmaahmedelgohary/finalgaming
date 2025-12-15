using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiraDialouge : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            string[] dialogue = {"Mira: You don not belong here.",
            "Zero: Then why does this place feels like it knows me.","Mira: Because you were... assembled not born.",
            "Zero: Assembled by who?","Mira: By someone who could not let go.."
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