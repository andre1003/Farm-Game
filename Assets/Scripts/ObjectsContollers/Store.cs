using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Store : MonoBehaviour {
    public GameObject storeCanvas;
    public Transform frontSpot;

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

    public void Buy(Plant plant) {
        PlayerDataManager.instance.Buy(plant);
    }
}
