using UnityEngine;


public class PauseMenu : MonoBehaviour {
    // UI
    public GameObject pauseCanvas;

    // Transition
    public float transitionLenght = 0.25f;


    // UI
    private CanvasGroup canvasGroup;
    private bool changeUI = false;

    // Transition
    private float initialAlpha;
    private float targetAlpha;
    private float elapsedTime = 0f;


    // Start method
    void Start()
    {
        canvasGroup = pauseCanvas.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update() {
        // If player press Esc key, pause or resume game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
        }

        // If doesn't need to change UI visibility, exit
        if(!changeUI)
        {
            return;
        }

        // Change UI opacity over transition lenght seconds
        canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, (elapsedTime / transitionLenght));
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
            }

            // Else, pause game
            else
            {
                Time.timeScale = 0f;
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
        Time.timeScale = 1f;
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
            Time.timeScale = 1f;
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
}
