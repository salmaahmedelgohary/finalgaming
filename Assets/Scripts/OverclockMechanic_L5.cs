using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Zero's special power - slow down time!
public class OverclockMechanic_L5 : MonoBehaviour
{
    [Header("Overclock Settings")]
    public KeyCode overclockKey = KeyCode.Q;
    public float slowMotionScale = 0.3f; // 30% speed = dramatic slow-mo
    public float maxOverclockDuration = 3f;
    public float rechargeRate = 1f; // How fast it recharges

    [Header("Visual Effects")]
    public Color overclockTint = new Color(0.3f, 0.7f, 1f, 0.3f); // Blue tint
    public GameObject overclockVFX; // Optional: particle effects

    private float overclockEnergy;
    private bool isOverclocked = false;
    private Camera mainCamera;

    void Start()
    {
        overclockEnergy = maxOverclockDuration;
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Activate overclock
        if (Input.GetKey(overclockKey) && overclockEnergy > 0 && !isOverclocked)
        {
            StartOverclock();
        }

        // Deactivate overclock
        if ((Input.GetKeyUp(overclockKey) || overclockEnergy <= 0) && isOverclocked)
        {
            StopOverclock();
        }

        // Drain energy while active
        if (isOverclocked)
        {
            overclockEnergy -= Time.unscaledDeltaTime; // Use unscaled time!
            if (overclockEnergy <= 0)
            {
                overclockEnergy = 0;
                StopOverclock();
            }
        }
        // Recharge when not active
        else if (overclockEnergy < maxOverclockDuration)
        {
            overclockEnergy += rechargeRate * Time.deltaTime;
            if (overclockEnergy > maxOverclockDuration)
                overclockEnergy = maxOverclockDuration;
        }
    }

    void StartOverclock()
    {
        isOverclocked = true;
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Keep physics stable

        // Visual effects
        if (overclockVFX != null)
            overclockVFX.SetActive(true);

        Debug.Log("🔥 OVERCLOCK ACTIVATED!");
    }

    void StopOverclock()
    {
        isOverclocked = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Visual effects
        if (overclockVFX != null)
            overclockVFX.SetActive(false);

        Debug.Log("⚡ Overclock ended");
    }

    void OnDestroy()
    {
        // Make sure time returns to normal when player is destroyed
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    // Public getters for UI
    public float GetOverclockPercent()
    {
        return overclockEnergy / maxOverclockDuration;
    }

    public bool IsOverclocked()
    {
        return isOverclocked;
    }
}