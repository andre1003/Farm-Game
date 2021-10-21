using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour {
    public LayerMask clickableArea;
    
    private NavMeshAgent myAgent;

    private void Start() {
        myAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(InGameSaves.GetCanMove() && Input.GetButtonDown("Fire1")) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo, 100, clickableArea)) {
                myAgent.SetDestination(hitInfo.point);
            }
        }
    }

    
}
