using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSettings", menuName = "ScriptableObjects/Settings")]
public class GameSettings : ScriptableObject
{
    // Game resolution
    public int resolutionWidth;
    public int resolutionHeight;
    public int refreshRate;

    // Fullscreen
    public bool fullscreen = true;

    // Audio volume
    public float generalVolume;

    // Graphics preset (low, medium, high, ultra)
    [Range(0, 5)] public int graphicsPreset;

    // Use grass
    public bool useAdditionalResource;
    

    /// <summary>
    /// Apply the saved settings
    /// </summary>
    public void ApplySettings()
    {
        // Set resolution and quality
        Screen.SetResolution(resolutionWidth, resolutionHeight, fullscreen, refreshRate);
        QualitySettings.SetQualityLevel(graphicsPreset);
    }
}
