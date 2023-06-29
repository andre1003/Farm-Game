using System.Collections.Generic;
using UnityEngine;


public class PauseMenu : MonoBehaviour {
    #region Singleton
    public static PauseMenu instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion


    // UI
    public GameObject pauseCanvas;
    public List<GameObject> allCanvas;

    // Transition
    public float transitionLength = 0.25f;


    // UI
    private CanvasGroup canvasGroup;
    private bool changeUI = false;
    private List<bool> canvasActiveSelf;

    // Transition
    private float initialAlpha;
    private float targetAlpha;
    private float elapsedTime = 0f;


    // Start method
    void Start()
    {
        // Get canvas group component
        canvasGroup = pauseCanvas.GetComponent<CanvasGroup>();

        // Set canvas active self list
        canvasActiveSelf = new List<bool>();
    }

    // Update is called once per frame
    void Update() {
        // If player press Esc key, pause or resume game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
        }

        // Change canvas opacity if needed
        ChangeCanvasOpacity();
    }

    /// <summary>
    /// Change canvas UI, if needed.
    /// </summary>
    private void ChangeCanvasOpacity()
    {
        // If doesn't need to change UI visibility, exit
        if(!changeUI)
        {
            return;
        }

        // Change UI opacity over transition length seconds
        canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, (elapsedTime / transitionLength));
        elapsedTime += Time.deltaTime;

        // If UI opacity is the target opacity, reset elapsed time and set change UI controller to false
        if(canvasGroup.alpha == targetAlpha)
        {
            elapsedTime = 0f;
            changeUI = false;

            // If target opacity is 0, set disable store canvas
            if(targetAlpha == 0f)
            {
                pauseCanvas.SetActive(false);
                ReactivateAllCanvas();
            }

            // Else, pause game
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Set UI visibility.
    /// </summary>
    /// <param name="isActive">New visibility.</param>
    private void SetUI(bool isActive)
    {
        // Set initial opacity
        initialAlpha = canvasGroup.alpha;

        // If the target is to display UI, set target opacity to 1
        if(isActive)
        {
            targetAlpha = 1f;
            DisableAllCanvas();
            pauseCanvas.SetActive(true);
            StopPlayerMovement(true);
        }

        // If the target is to hide UI, set target opacity to 0
        else
        {
            targetAlpha = 0f;
            StopPlayerMovement(false);
        }

        // Handle player busy
        InGameSaves.ChangeIsBusy();

        // Set change UI controller to true
        changeUI = true;
    }

    /// <summary>
    /// Close pause menu.
    /// </summary>
    public void Close() {
        // Change HUD and pause menun cavases visibility
        HUDManager.instance.SetHUD(true);
        SetUI(false);

        // Resume game
        ResumeGame();
    }

    /// <summary>
    /// Quit game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Pause or resume game.
    /// </summary>
    private void PauseResume()
    {
        // Resume game
        if(pauseCanvas.activeSelf)
        {
            ResumeGame();
        }

        // Change HUD and pause menu cavases visibilities
        HUDManager.instance.SetHUD(pauseCanvas.activeSelf);
        SetUI(!pauseCanvas.activeSelf);
    }

    /// <summary>
    /// Handle player movement.
    /// </summary>
    private void StopPlayerMovement(bool stop) {
        // If game is paused, stop moving
        if(stop)
        {
            MovementController.instance.StopPlayer();
        }

        // If resume game, start moving
        else
        {
            MovementController.instance.ResumePlayer();
        }
    }

    /// <summary>
    /// Pause game completly.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    /// <summary>
    /// Resume game completly.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// Disable all canvases.
    /// </summary>
    private void DisableAllCanvas()
    {
        // Clear canvas active self list and loop all canvas
        canvasActiveSelf.Clear();
        for(int i = 0; i < allCanvas.Count; i++)
        {
            // Add canvas active self to list
            canvasActiveSelf.Add(allCanvas[i].activeSelf);

            // If it is inventory canvas, call smooth change UI
            if(allCanvas[i].name.Contains("Inventory"))
            {
                InventoryUI.instance.SetUI(false);
            }

            // If it is store canvas, call smooth change UI
            else if(allCanvas[i].name.Contains("Store"))
            {
                Store.instance.SetUI(false);
            }

            // Else, set canvas active self to false
            else
            {
                allCanvas[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Reactivate all canvases.
    /// </summary>
    private void ReactivateAllCanvas()
    {
        // Reactivate all needed canvas
        for(int i = 0; i < allCanvas.Count; i++)
        {
            // If it is inventory canvas, call smooth change UI
            if(allCanvas[i].name.Contains("Inventory"))
            {
                InventoryUI.instance.SetUI(canvasActiveSelf[i]);
            }

            // If it is store canvas, call smooth change UI
            else if(allCanvas[i].name.Contains("Store"))
            {
                Store.instance.SetUI(canvasActiveSelf[i]);
            }

            // Else, set canvas active self to previous state
            else
            {
                allCanvas[i].SetActive(canvasActiveSelf[i]);
            }
        }
    }
}
