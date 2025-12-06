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
            string[] dialogue = {"hhhhhhhhhhhhhhhhhhhhhhhh",
            "ddddddddddddddddddddddddddddddddddd","ggggggggggggggggggggggggggggggg",
            "g=oooooooooooooooooooooooooo","fffffffffffffffffffffffffffffff"
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
