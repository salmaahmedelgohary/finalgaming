using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    public AudioSource voiceSource;   // drag the AudioSource here
    private bool hasPlayed = false;   // optional: only once

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            if (voiceSource != null)
            {
                voiceSource.Play();   // play the father’s voice
                hasPlayed = true;     // remove this line if you want it every time
            }
        }
    }
}
