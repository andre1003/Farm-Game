using UnityEngine;


public class PauseMenu : MonoBehaviour {
    public GameObject pauseCanvas;
    public GameObject HUDCanvas;


    // Update is called once per frame
    void Update() {
        // If player press Esc key, pause or resume game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
        }
    }

    /// <summary>
    /// Close pause menu.
    /// </summary>
    public void Close() {
        // Change HUD and pause menun cavases visibility
        HUDCanvas.SetActive(true);
        pauseCanvas.SetActive(false);

        // Handle player busy and movement
        InGameSaves.ChangeIsBusy();
        HandlePlayer();
    }

    /// <summary>
    /// Pause or resume game.
    /// </summary>
    private void PauseResume()
    {
        // Change HUD and pause menun cavases visibility
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
        HUDCanvas.SetActive(!HUDCanvas.activeSelf);

        // Handle player busy and movement
        InGameSaves.ChangeIsBusy();
        HandlePlayer();
    }

    /// <summary>
    /// Handle player movement.
    /// </summary>
    private void HandlePlayer() {
        // If game is paused, stop moving
        if(pauseCanvas.activeSelf)
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
