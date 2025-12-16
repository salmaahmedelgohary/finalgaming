using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Level6")]
    public string nextSceneName;

    [Header("Visual Settings")]
    public Color inactiveColor = Color.yellow;
    public Color activeColor = Color.green;
    public GameObject effectPrefab;  

    private SpriteRenderer spriteRenderer;
    private bool isActivated = false;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = inactiveColor;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
     
        if (spriteRenderer != null)
        {
            spriteRenderer.color = activeColor;
        }

        
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }

        Debug.Log("Loading scene: " + nextSceneName);

       
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty! Please enter a scene name.");
        }
    }

    void LoadNextSceneWithDelay(float delay)
    {
        Invoke("LoadNextScene", delay);
    }
}