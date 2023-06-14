using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    #region Attributes
    [Header("Game settings")]
    // Game settings
    public GameSettings gameSettings;

    [Space(15)]
    [Header("Canvases")]
    // Canvases
    public GameObject loadCanvas;
    public GameObject settingsCanvas;
    public GameObject mainMenuCanvas;

    [Space(15)]
    [Header("Progress bar")]
    // Progress bar
    public Slider progressBar;

    [Space(15)]
    [Header("Settings")]
    // Settings
    public TextMeshProUGUI resolutionText;
    public TextMeshProUGUI graphicsText;
    public TextMeshProUGUI languageText;
    public Toggle fullscreenToggle;
    public Toggle additionalResourcesToggle;

    [Space(15)]
    [Header("Audio")]
    // Audio
    public AudioSource audioSource;
    public AudioClip click;
    public AudioClip hover;
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    [Space(15)]
    [Header("Post process")]
    // Post process
    public PostProcessVolume postProcess;
    private DepthOfField depthOfField;

    [Space(15)]
    [Header("Cursor")]
    // Cursor
    public Texture2D normalCursor;
    public Texture2D hoverCursor;
    public Vector2 hotSpot = Vector2.zero;


    // Settings
    private int selectedResolution;
    private int selectedGraphics;
    private int selectedLanguage;

    // Options
    private List<string> resolutionOptions;
    private List<string> graphicsOptions = new List<string>
        {
            "Very Low",
            "Low",
            "Medium",
            "High",
            "Very High",
            "Ultra"
        };
    #endregion

    #region Unity Methods
    void Awake()
    {
        // If the game settings are not configured yet
        if(gameSettings.resolutionWidth == 0f &&  gameSettings.resolutionHeight == 0f)
        {
            // Set resolution
            gameSettings.resolutionWidth = Screen.currentResolution.width;
            gameSettings.resolutionHeight = Screen.currentResolution.height;
            gameSettings.refreshRate = Screen.currentResolution.refreshRate;
        }

        // Apply game settings
        gameSettings.ApplySettings();

        // Add all possible resolutions to an list
        Resolution[] resolutions = Screen.resolutions;
        resolutionOptions = new List<string>();
        for(int i = 0; i < resolutions.Length; i++)
        {
            // Only add if the resolution is unique
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if(!resolutionOptions.Contains(option))
            {
                resolutionOptions.Add(option);
            }
        }

        // Get the selected resolution and update the settings text
        selectedResolution = resolutionOptions.IndexOf(gameSettings.resolutionWidth + " x " + gameSettings.resolutionHeight);
        resolutionText.text = resolutionOptions[selectedResolution];

        // Get the fullscreen and useAdditionalResource values and update the settings texts
        fullscreenToggle.isOn = gameSettings.fullscreen;
        additionalResourcesToggle.isOn = gameSettings.useAdditionalResource;

        // Get the graphics preset and update settings text
        selectedGraphics = gameSettings.graphicsPreset;
        StringLocalizer.instance.Localize("Graphics Text", graphicsOptions[selectedGraphics], graphicsText);

        // Get all localization locales
        StringLocalizer.instance.GetLocales();
    }

    /// <summary>
    /// Unity Start method.
    /// </summary>
    void Start()
    {
        // Get DepthOfField from post process volume
        postProcess.profile.TryGetSettings<DepthOfField>(out depthOfField);

        // Change volume
        volumeSlider.value = gameSettings.generalVolume;
        SetVolume(gameSettings.generalVolume);
    }
    #endregion

    #region Main Menu Functionalities
    /// <summary>
    /// Play game method.
    /// </summary>
    public void Play()
    {
        // Play click sound and start screen blur
        ClickSound();
        StartCoroutine(LoadGame(0.5f, 5f, 2f));
    }

    private IEnumerator LoadAsyncScene(int index)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // Wait until the asynchronous scene fully loads
        while(!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            progressBar.value = progress;

            yield return null;
        }
    }

    /// <summary>
    /// Quit the game.
    /// </summary>
    public void QuitGame()
    {
        // Play click sound and quit game
        ClickSound();
        Application.Quit();
    }

    /// <summary>
    /// Called when mouse cursor is hovering a button.
    /// </summary>
    public void HoverCursor()
    {
        // Set cursor to hoverCursor and play hover sound
        Cursor.SetCursor(hoverCursor, hotSpot, CursorMode.Auto);
        audioSource.clip = hover;
        audioSource.Play();
    }

    /// <summary>
    /// Called when cursor stop hovering a button.
    /// </summary>
    public void NormalCursor()
    {
        // Set cursor to normalCursor
        Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
    }

    /// <summary>
    /// Play click sound.
    /// </summary>
    public void ClickSound()
    {
        audioSource.clip = click;
        audioSource.Play();
    }

    /// <summary>
    /// Blur the screen in seconds and load the game.
    /// </summary>
    /// <param name="seconds">Seconds to finish bluring.</param>
    /// <param name="initialFocusDistance">Initial focus distance.</param>
    /// <param name="finalFocusDistance">Final focus distance.</param>
    private IEnumerator LoadGame(float seconds, float initialFocusDistance, float finalFocusDistance)
    {
        // Disable the main menu canvas
        mainMenuCanvas.SetActive(false);

        // Start timer, while elapsed time is less than seconds
        float elapsedTime = 0f;
        while(elapsedTime < seconds)
        {
            // Change depth of field focus distance, add deltaTime to elapsedTime and yield return null
            depthOfField.focusDistance.Override(Mathf.Lerp(initialFocusDistance, finalFocusDistance, (elapsedTime / seconds)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Activate load canvas
        loadCanvas.SetActive(true);

        // Get next scene index and start loading it async
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(LoadAsyncScene(index));
    }

    /// <summary>
    /// Apply blur to the screen.
    /// </summary>
    public void ApplyBlur()
    {
        // Play click sound, set cursor to normal and apply the blur
        ClickSound();
        NormalCursor();
        StartCoroutine(BlurInSeconds(0.5f, 5f, 2f));
    }

    /// <summary>
    /// Remove blur from the screen.
    /// </summary>
    public void RemoveBlur()
    {
        // Play click sound, set cursor to normal and remove the blur
        ClickSound();
        NormalCursor();
        StartCoroutine(BlurInSeconds(0.5f, 2f, 5f));
    }

    /// <summary>
    /// Blur the screen in seconds.
    /// </summary>
    /// <param name="seconds">Seconds to finish bluring.</param>
    /// <param name="initialFocusDistance">Initial focus distance.</param>
    /// <param name="finalFocusDistance">Final focus distance.</param>
    private IEnumerator BlurInSeconds(float seconds, float initialFocusDistance, float finalFocusDistance)
    {
        // Start timer, while elapsed time is less than seconds
        float elapsedTime = 0f;
        while(elapsedTime < seconds)
        {
            // Change depth of field focus distance, add deltaTime to elapsedTime and yield return null
            depthOfField.focusDistance.Override(Mathf.Lerp(initialFocusDistance, finalFocusDistance, (elapsedTime / seconds)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region Settings Functionalities
    /// <summary>
    /// Set general volume.
    /// </summary>
    /// <param name="volume">Volume value.</param>
    public void SetVolume(float volume)
    {
        gameSettings.generalVolume = volume;
        audioMixer.SetFloat("GeneralVolume", volume);
    }

    /// <summary>
    /// Set graphics preset quality.
    /// </summary>
    /// <param name="quality">Index of graphics preset.</param>
    public void SetGraphicsPreset(int quality)
    {
        // Clamp the quality to make sure that it's between 0 and 5, and save
        quality = Mathf.Clamp(0, 5, quality);
        gameSettings.graphicsPreset = quality;
    }

    /// <summary>
    /// Apply all settings.
    /// </summary>
    public void ApplySettings()
    {
        // Get the selected resolution from resolutionOptions
        string resolution = resolutionOptions[selectedResolution];
        int width = int.Parse(resolution.Split(" x ")[0]);
        int height = int.Parse(resolution.Split(" x ")[1]);

        // Update resolution in game settings 
        gameSettings.resolutionWidth = width;
        gameSettings.resolutionHeight = height;

        // Update graphics in game settings
        gameSettings.graphicsPreset = selectedGraphics;

        gameSettings.languageIndex = selectedLanguage;

        // Apply the settings
        gameSettings.ApplySettings();

        // Update all localized strings via C#
        UpdateLocalizedStrings();

        // Play click sound
        ClickSound();
    }

    /// <summary>
    /// Select next resolution from resolution options.
    /// If the current selected resolution is the last, it goes back to beginning.
    /// </summary>
    public void NextResolution()
    {
        // Play click sound
        ClickSound();

        // Increase the selected resolution and check if it is out of bounds. If it is, go back to beginning
        selectedResolution++;
        if(selectedResolution >= resolutionOptions.Count)
        {
            selectedResolution = 0;
        }

        // Update resolution text
        resolutionText.text = resolutionOptions[selectedResolution];
    }

    /// <summary>
    /// Select previous resolution from resolution options.
    /// If the current selected resolution is the first, it goes back to the ending.
    /// </summary>
    public void PrevResolution()
    {
        // Play click sound
        ClickSound();

        // Decrease the selected resolution and check if it is out of bounds. If it is, go back to the ending
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = resolutionOptions.Count - 1;
        }

        // Update resolution text
        resolutionText.text = resolutionOptions[selectedResolution];
    }

    /// <summary>
    /// Select next graphic setting from graphics options.
    /// If the current selected graphics is the last, it goes back to beginning.
    /// </summary>
    public void NextGraphics()
    {
        // Play click sound
        ClickSound();

        // Increase the selected graphic settings and check if it is out of bounds. If it is, go back to beginning
        selectedGraphics++;
        if(selectedGraphics >= graphicsOptions.Count)
        {
            selectedGraphics = 0;
        }

        // Update graphics text
        StringLocalizer.instance.Localize("Graphics Text", graphicsOptions[selectedGraphics], graphicsText);
    }

    /// <summary>
    /// Select previous graphic setting from graphics options.
    /// If the current selected graphic is the first, it goes back to the ending.
    /// </summary>
    public void PrevGraphics()
    {
        // Play click sound
        ClickSound();

        // Decrease the selected graphic settings and check if it is out of bounds. If it is, go back to the ending
        selectedGraphics--;
        if(selectedGraphics < 0)
        {
            selectedGraphics = graphicsOptions.Count - 1;
        }

        // Update graphics text
        StringLocalizer.instance.Localize("Graphics Text", graphicsOptions[selectedGraphics], graphicsText);
    }

    /// <summary>
    /// Select next language available.
    /// If the current selected language is the last, it goes back to beginning.
    /// </summary>
    public void NextLanguage()
    {
        // Play click sound
        ClickSound();

        // Increase the selected language and check if it is out of bounds. If it is, go back to beginning
        selectedLanguage++;
        if(selectedLanguage >= StringLocalizer.instance.options.Count)
        {
            selectedLanguage = 0;
        }

        // Update language text
        languageText.text = StringLocalizer.instance.options[selectedLanguage];
    }

    /// <summary>
    /// Select previous language available.
    /// If the current selected language is the first, it goes back to the ending.
    /// </summary>
    public void PrevLanguage()
    {
        // Play click sound
        ClickSound();

        // Decrease the selected language and check if it is out of bounds. If it is, go back to the ending
        selectedLanguage--;
        if(selectedLanguage < 0)
        {
            selectedLanguage = StringLocalizer.instance.options.Count - 1;
        }

        // Update language text
        languageText.text = StringLocalizer.instance.options[selectedLanguage];
    }

    /// <summary>
    /// Change fullscreen option.
    /// </summary>
    /// <param name="isOn">New fullscreen value.</param>
    public void ChangeFullscreen(bool isOn)
    {
        gameSettings.fullscreen = isOn;
    }

    /// <summary>
    /// Change the use of additional resource.
    /// </summary>
    /// <param name="useAdditionalResource">New use of additional resource value.</param>
    public void ChangeUseAdditionalResource(bool useAdditionalResource)
    {
        gameSettings.useAdditionalResource = useAdditionalResource;
    }

    /// <summary>
    /// Update all localized strings via C# script.
    /// This method waits for localization settings initialize,
    /// so it can work propertly.
    /// </summary>
    public void UpdateLocalizedStrings()
    {
        StartCoroutine(UpdateStrings());
    }

    /// <summary>
    /// Wait localization settings initialization and update strings.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateStrings()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;
        StringLocalizer.instance.Localize("Graphics Text", graphicsOptions[selectedGraphics], graphicsText);
    }

    /// <summary>
    /// Update the current language based on game settings.
    /// </summary>
    public void UpdateLanguage()
    {
        // Change language
        selectedLanguage = gameSettings.languageIndex;
        languageText.text = StringLocalizer.instance.options[selectedLanguage];
    }
    #endregion
}
