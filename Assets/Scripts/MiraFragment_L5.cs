using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mira's fragments appear silently to guide Zero
public class MiraFragment_L5 : MonoBehaviour
{
    [Header("Fragment Settings")]
    public float floatSpeed = 1f;
    public float floatAmount = 0.3f;
    public float fadeInDuration = 1f;
    public float stayDuration = 3f; // How long before moving to next position
    public Transform nextFragmentPosition; // Where this fragment leads to

    [Header("Visual")]
    public Color fragmentColor = new Color(0.5f, 0.8f, 1f, 0.8f); // Light blue
    public GameObject fragmentVFX; // Particle effects

    private SpriteRenderer sprite;
    private Vector3 startPosition;
    private float timeAlive = 0f;
    private bool isMovingToNext = false;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        // Start invisible
        Color c = sprite.color;
        c.a = 0;
        sprite.color = c;

        // Fade in
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        // Floating animation
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // After staying for a while, move to next position
        if (timeAlive > stayDuration && !isMovingToNext && nextFragmentPosition != null)
        {
            isMovingToNext = true;
            StartCoroutine(MoveToNext());
        }
    }

    System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color targetColor = fragmentColor;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, targetColor.a, elapsed / fadeInDuration);
            Color c = targetColor;
            c.a = alpha;
            sprite.color = c;
            yield return null;
        }

        sprite.color = targetColor;
    }

    System.Collections.IEnumerator MoveToNext()
    {
        // Fade out
        float elapsed = 0f;
        Color startColor = sprite.color;

        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0, elapsed / 0.5f);
            Color c = startColor;
            c.a = alpha;
            sprite.color = c;
            yield return null;
        }

        // Move to next position and fade back in
        if (nextFragmentPosition != null)
        {
            transform.position = nextFragmentPosition.position;
            startPosition = transform.position;
            timeAlive = 0f;
            isMovingToNext = false;
            StartCoroutine(FadeIn());
        }
        else
        {
            // No more positions, disappear
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player reached this fragment
            Debug.Log("✨ Fragment reached - leading to next position");

            // Create next fragment if specified
            if (nextFragmentPosition != null)
            {
                isMovingToNext = true;
                StartCoroutine(MoveToNext());
            }
        }
    }
}