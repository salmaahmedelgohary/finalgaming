using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //need to use this namespace to be able to utilise the TextMesh Pro data types and functions

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay; //a special variable that holds the TextMeshPro - Text for manipulation
private string[] dialogueSentences; //an array that stores all the sentences to be displayed
private int index = 0; //a variable that signifies which sentence is being printed or to be printed
public float typingSpeed; //a variable to control the speed of the typewriter effect
public GameObject continueButton; //a variable that holds the continue button
public GameObject dialogueBox; //a variable that holds the panel (dialogue box)
public Rigidbody2D playerRB; //a variable that holds the player's/character's Rigidbody2D component
    // Start is called before the first frame update
    void Start()
    {
       dialogueBox.SetActive(false);
        continueButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSentences(string[] sentences){
    this.dialogueSentences = sentences;

    }

    //IEnumerator is a special type of function where with the help of the StartCoroutine() function, the
//IEnumerator can be paused and resumed for a specified amount of time without pausing the game itself.
//The purpose of this function is to display the dialogue with a typewriter effect
public IEnumerator TypeDialogue(){
    if (dialogueSentences == null || index < 0 || index >= dialogueSentences.Length)
    {
        yield break;
    }

    dialogueBox.SetActive(true); //enables the dialogue box
    continueButton.SetActive(false);
    // freeze the whole scene (use Time.timeScale) while keeping typing coroutine running via realtime waits
    Time.timeScale = 0f;
    playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

    textDisplay.text = "";
    foreach (char letter in dialogueSentences[index].ToCharArray())
    {
        textDisplay.text += letter;
        yield return new WaitForSecondsRealtime(typingSpeed);
    }

    continueButton.SetActive(true);
}
public void NextSentence(){
    Debug.Log("Inside NextSentence");
    if(index < dialogueSentences.Length - 1){

        index++;
        textDisplay.text = "";
        StartCoroutine(TypeDialogue());


    }
    else{
    
        textDisplay.text = "";
        dialogueBox.SetActive(false);
        this.dialogueSentences = null;
        index = 0;
        // unfreeze scene and restore player constraints
        Time.timeScale = 1f;
        playerRB.constraints = RigidbodyConstraints2D.None;
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        continueButton.SetActive(false);
        



    }





}
}
