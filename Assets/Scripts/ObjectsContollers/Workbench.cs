using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Workbench : MonoBehaviour {
    public GameObject workbenchCanvas;
    public Transform frontSpot;

    private NavMeshAgent myAgent;

    // Method for closing the workbench canvas
    public void Close() {
        InGameSaves.ChangeCanMove();
        InGameSaves.ChangeIsBusy();
        workbenchCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        InGameSaves.ChangeCanMove();
        InGameSaves.ChangeIsBusy();
        workbenchCanvas.SetActive(true);
        myAgent = other.GetComponent<NavMeshAgent>();
        myAgent.SetDestination(frontSpot.position);
    }
}
