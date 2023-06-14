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

    // Graphics preset (very low, low, medium, high, very high, ultra)
    [Range(0, 5)] public int graphicsPreset;

    // Use additional resource
    public bool useAdditionalResource;

    // Selected language
    public int languageIndex;
    

    /// <summary>
    /// Apply the saved settings.
    /// </summary>
    public void ApplySettings()
    {
        // Set resolution
        Screen.SetResolution(resolutionWidth, resolutionHeight, fullscreen, refreshRate);

        // Set quality
        QualitySettings.SetQualityLevel(graphicsPreset);

        // Set game language
        StringLocalizer.instance.ChangeLanguage(languageIndex);
    }
}
