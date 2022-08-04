using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour {
    // Canvases
    public GameObject driveCanvas;
    public GameObject tractorOptions;

    // Spots
    public GameObject driveSpot;
    public GameObject leaveSpot;


    // Driving controller
    private bool isDriving = false;

    // Player reference
    private GameObject player = null;


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // If not driving and drive canvas is not active, exti Update method
        if(!isDriving && !driveCanvas.activeSelf)
            return;

        // If player is not driving
        if(!isDriving) {
            // If E key pressed
            if(Input.GetKeyDown(KeyCode.E)) {
                // Close drive canvas
                driveCanvas.SetActive(false);

                // Set driving animation and deactivate player agent
                MovementController.instance.SetAnimation("isDriving", true);
                MovementController.instance.SetMyAgentActive(false);

                // After 0.4 seconds, change player position (this prevent player from poping up inside tractor)
                StartCoroutine(Wait(0.4f));

                // Set player rotation
                player.transform.rotation = driveSpot.transform.rotation;
                
                // Open tractor options canvas
                tractorOptions.SetActive(true);

                // Change player busy
                InGameSaves.ChangeIsBusy();

                // Set driving controller
                isDriving = true;
            }
        }
        // If player is driving
        else {
            // If E key pressed
            if(Input.GetKeyDown(KeyCode.E)) {
                // Open tractor options canvas
                tractorOptions.SetActive(false);

                // Change player position
                player.transform.position = leaveSpot.transform.position;

                // Set driving animation and activate player agent
                MovementController.instance.SetAnimation("isDriving", false);
                MovementController.instance.SetMyAgentActive(true);

                // Change player busy
                InGameSaves.ChangeIsBusy();

                // Set driving controller
                isDriving = false;
            }
        }
    }

    // Wait for seconds and then change player position method
    private IEnumerator Wait(float seconds) {
        yield return new WaitForSeconds(seconds);

        player.transform.position = driveSpot.transform.position;
    }

    // On trigger enter event method
    private void OnTriggerEnter(Collider other) {
        // If collides with player
        if(other.gameObject.tag == "Player") {
            // Set drive canvas to true and player to other.gameObject
            driveCanvas.SetActive(true);
            player = other.gameObject;
        }
    }

    // On trigger exit event method
    private void OnTriggerExit(Collider other) {
        // If collides with player
        if(other.gameObject.tag == "Player") {
            // Set drive canvas to false
            driveCanvas.SetActive(false);
        }
    }

    public void Harvest() {
        // Maybe it could be a good idea to use Dijkstra algorithm to find the best path to harvest.
    }
}
