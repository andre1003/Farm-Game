using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Workbench : MonoBehaviour
{
    // Canvas
    public GameObject workbenchCanvas;

    // Destination
    public Transform frontSpot;

    // Navigation mesh agent reference
    private NavMeshAgent myAgent;


    /// <summary>
    /// Method for closing the workbench canvas.
    /// </summary>
    public void Close()
    {
        workbenchCanvas.SetActive(false);
        InGameSaves.ChangeIsBusy();
    }

    // Method for Trigger Enter
    private void OnTriggerEnter(Collider other)
    {
        // Set workbench canvas to true
        workbenchCanvas.SetActive(true);

        // Get player agent and stop animation
        myAgent = other.GetComponent<NavMeshAgent>();
        EndAnimationHandler(myAgent);

        // Chang player busy state
        InGameSaves.ChangeIsBusy();
    }

    /// <summary>
    /// Handler for walking animation.
    /// </summary>
    /// <param name="myAgent">Navigation mesh event.</param>
    private void EndAnimationHandler(NavMeshAgent myAgent)
    {
        // Set destination
        myAgent.SetDestination(frontSpot.position);

        // I need the time variation that player will take when him/she enter at
        // Store zone. So, for that, I'll need deltaTime = (deltaSpace) / playerSpeed.

        // Calculate the path
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(frontSpot.position, myAgent.transform.position, myAgent.areaMask, path);

        // Calculate the distance, considering all corners from path
        float distance = 0f;
        for(int i = 0; i < path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }

        // Calculating deltaTime, form physics formula
        float deltaTime = distance / myAgent.speed;

        // Coroutine for ending animation
        StartCoroutine(Wait(deltaTime));
    }

    /// <summary>
    /// Method for correctly end animation.
    /// </summary>
    /// <param name="seconds">Seconds to wait.</param>
    private IEnumerator Wait(float seconds)
    {
        // Wait for a given seconds
        yield return new WaitForSeconds(seconds);

        // Stop walking
        MovementController.instance.animator.SetBool("isWalking", false);
    }
}
