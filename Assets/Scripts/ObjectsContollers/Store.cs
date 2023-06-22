using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class Store : MonoBehaviour
{
    #region Sigleton
    public static Store instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Store found!");
            return;
        }

        instance = this;
    }
    #endregion

    // Canvas
    public GameObject storeCanvas;

    // Destination
    public Transform frontSpot;

    // Store plants
    public Plant[] storePlants;


    // Navigation mesh event
    private NavMeshAgent myAgent;


    /// <summary>
    /// Method for closing the workbench canvas.
    /// </summary>
    public void Close()
    {
        storeCanvas.SetActive(false);
        InGameSaves.ChangeIsBusy();
    }

    // Method for Trigger Enter
    private void OnTriggerEnter(Collider other)
    {
        // Set store canvas visibility to true
        storeCanvas.SetActive(true);

        // Get player agent and stop animation
        myAgent = other.GetComponent<NavMeshAgent>();
        EndAnimationHandler(myAgent);

        // Chang player busy state
        InGameSaves.ChangeIsBusy();

        // Get tutorial
        TutorialManager.instance.GetTutorial("store");
    }

    /// <summary>
    /// Handler for walking animation.
    /// </summary>
    /// <param name="myAgent">Navigation mesh agent.</param>
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
        yield return new WaitForSeconds(seconds);
        MovementController.instance.animator.SetBool("isWalking", false);
    }
}
