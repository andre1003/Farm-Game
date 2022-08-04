using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public GameObject pauseCanvas;
    public GameObject HUDCanvas;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape) && !InGameSaves.GetIsBusy()) {
            pauseCanvas.SetActive(!pauseCanvas.activeSelf);
            HUDCanvas.SetActive(!HUDCanvas.activeSelf);
            InGameSaves.ChangeIsBusy();
            HandlePlayer();
        }
    }

    public void Close() {
        HUDCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        InGameSaves.ChangeIsBusy();
        HandlePlayer();
    }

    private void HandlePlayer() {
        if(pauseCanvas.activeSelf)
            MovementController.instance.StopPlayer();
        else
            MovementController.instance.ResumePlayer();
    }
}
