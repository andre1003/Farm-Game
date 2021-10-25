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
        workbenchCanvas.SetActive(false);
        InGameSaves.ChangeIsBusy();
    }

    // Method for Trigger Enter
    private void OnTriggerEnter(Collider other) {
        workbenchCanvas.SetActive(true);

        myAgent = other.GetComponent<NavMeshAgent>();
        EndAnimationHandler(myAgent);

        InGameSaves.ChangeIsBusy();
    }

    // Handler for walking animation
    private void EndAnimationHandler(NavMeshAgent myAgent) {
        myAgent.SetDestination(frontSpot.position);

        // I need the time variation that player will take when him/she enter at
        // Store zone. So, for that, I'll need deltaTime = (deltaSpace) / playerSpeed.

        // Calculate the path
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(frontSpot.position, myAgent.transform.position, myAgent.areaMask, path);

        // Calculate the distance, considering all corners from path
        float distance = 0f;
        for(int i = 0; i < path.corners.Length - 1; i++) {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        // Calculating deltaTime, form physics formula
        float deltaTime = distance / myAgent.speed;

        // Coroutine for ending animation
        StartCoroutine(Wait(deltaTime));
    }

    // Method for correctly end animation
    private IEnumerator Wait(float seconds) {
        yield return new WaitForSeconds(seconds);
        MovementController.instance.animator.SetBool("isWalking", false);
    }
}
