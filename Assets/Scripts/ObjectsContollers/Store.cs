using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Store : MonoBehaviour {
    #region Sigleton
    public static Store instance;

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of Store found!");
            return;
        }

        instance = this;
    }
    #endregion

    public GameObject storeCanvas;
    public Transform frontSpot;

    public Plant[] storePlants;

    private NavMeshAgent myAgent;

    // Method for closing the workbench canvas
    public void Close() {
        storeCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        storeCanvas.SetActive(true);
        myAgent = other.GetComponent<NavMeshAgent>();
        myAgent.SetDestination(frontSpot.position);
    }
}
