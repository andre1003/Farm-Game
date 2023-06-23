using System.Collections;
using System.Collections.Generic;
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
    public GameObject newItemCanvas;

    // Destination
    public Transform frontSpot;

    // Store plants
    public Plant[] storePlants;


    // Navigation mesh event
    private NavMeshAgent myAgent;

    // Sold plants
    private Dictionary<string, int> soldPlants = new Dictionary<string, int>();

    private bool newLevel = true;


    void Update()
    {
        newItemCanvas.SetActive(newLevel);
    }

    /// <summary>
    /// Method for closing the store canvas.
    /// </summary>
    public void Close()
    {
        storeCanvas.SetActive(false);
        InGameSaves.ChangeIsBusy();
        TutorialManager.instance.PauseTutorial();
    }

    // Method for Trigger Enter
    private void OnTriggerEnter(Collider other)
    {
        // Update store menu UI and set store canvas visibility to true
        StoreUI.instance.UpdateMenu();
        storeCanvas.SetActive(true);

        // Get player agent and stop animation
        myAgent = other.GetComponent<NavMeshAgent>();
        EndAnimationHandler(myAgent);        

        // Chang player busy state
        InGameSaves.ChangeIsBusy();

        // Set new level controller to false
        newLevel = false;

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

    /// <summary>
    /// Add a plant to sold plants dictionary.
    /// </summary>
    /// <param name="name">Plant name, used as the dictionary key.</param>
    /// <param name="amount">Sold amount.</param>
    public void AddSoldPlant(string name, int amount)
    {
        // Make sure the key is lower case
        name = name.ToLower();

        // If this plant already exists in the dictionary, add the amount
        if(soldPlants.ContainsKey(name))
        {
            soldPlants[name] += amount;
        }

        // If the key is invalid, add it to dictionary
        else
        {
            soldPlants.Add(name, amount);
        }
    }

    /// <summary>
    /// Check if player sold a certain amount of a plant.
    /// </summary>
    /// <param name="name">Plant name to check.</param>
    /// <param name="amount">Sold amount to check.</param>
    /// <returns>TRUE - If it did. FALSE - If it didn't.</returns>
    public bool HasSold(string name, int amount=-1)
    {
        // Make sure the key is lower case
        name = name.ToLower();

        // If the plant has not been sold, return false
        if(!soldPlants.ContainsKey(name))
        {
            return false;
        }

        // If the amount is invalid, return true
        if(amount == -1)
        {
            return true;
        }

        // If the amount is valid, return if the amount of sold plant is bigger
        // or equal to the given amount
        return soldPlants[name] >= amount;
    }

    /// <summary>
    /// Clear all sold plants.
    /// </summary>
    public void ClearSoldPlants()
    {
        soldPlants.Clear();
    }

    /// <summary>
    /// Check for new items on store.
    /// </summary>
    public void CheckNewItems()
    {
        int playerLevel = PlayerDataManager.instance.playerData.level - 1;
        foreach(Plant plant in storePlants)
        {
            if(playerLevel == plant.levelRequired)
            {
                newLevel = true;
                return;
            }
        }
    }
}
