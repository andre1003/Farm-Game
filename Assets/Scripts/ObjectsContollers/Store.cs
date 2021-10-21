using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Store : MonoBehaviour {
    public GameObject storeCanvas;
    public Transform frontSpot;

    public Text moneyText;

    private NavMeshAgent myAgent;

    // Method for closing the workbench canvas
    public void Close() {
        InGameSaves.ChangeCanMove();
        InGameSaves.ChangeIsBusy();
        storeCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        InGameSaves.ChangeCanMove();
        InGameSaves.ChangeIsBusy();
        storeCanvas.SetActive(true);
        myAgent = other.GetComponent<NavMeshAgent>();
        myAgent.SetDestination(frontSpot.position);
    }

    public void Buy(Plant plant) {
        PlayerDataManager.Buy(plant);
        moneyText.text = PlayerDataManager.GetMoney();
    }
}
