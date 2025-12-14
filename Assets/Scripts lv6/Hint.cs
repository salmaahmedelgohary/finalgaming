using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public Dialogue dialogueManager; // a variable that stores the Dialogue script that is attached to the Dialogue Manager GameObject

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") // if the player is the one that triggers the collider, then
        {
            string[] dialogue = { "Press S to anchor moving tiles" };

            dialogueManager.SetSentences(dialogue); // set the sentences array in the Dialogue script to above array
            dialogueManager.StartCoroutine(dialogueManager.TypeDialogue()); // start the coroutine of TypeDialogue()

            Destroy(GetComponent<BoxCollider2D>(), 0.2f); // destroys the NPC's triggered box collider so the player doesn't accidentally re-trigger
            Destroy(gameObject, 0.2f); // destroy the NPC itself after 5 seconds
        }
    }
}
