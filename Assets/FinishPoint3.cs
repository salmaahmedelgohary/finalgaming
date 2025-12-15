using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint3 : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (SceneController3.instance != null)
            {
                SceneController3.instance.NextLevel();
            }
            else
            {
                Debug.LogError("FinishPoint3: SceneController3.instance is not assigned. Make sure a SceneController3 component exists in the scene.");
            }
        }
    }
}
