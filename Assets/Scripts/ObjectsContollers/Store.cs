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

    // Transition
    public float transitionLength = 0.25f;

    // Destination
    public Transform frontSpot;

    // Store items
    public Item[] storeItems;


    // Navigation mesh event
    private NavMeshAgent myAgent;

    // Sold items
    private Dictionary<string, int> soldItems = new Dictionary<string, int>();

    // Items
    private bool newItems = true;
    private Item selectedItem;
    private StoreSlot selectedSlot;

    // UI
    private CanvasGroup canvasGroup;
    private bool changeUI = false;

    // Transition
    private float initialAlpha;
    private float targetAlpha;
    private float elapsedTime = 0f;


    // Start method
    void Start()
    {
        canvasGroup = storeCanvas.GetComponent<CanvasGroup>();
    }

    // Update method
    void Update()
    {
        // Update new items canvas active status based on newItems variable
        newItemCanvas.SetActive(newItems);

        // Change canvas opacity, if needed
        ChangeCanvasOpacity();
    }

    /// <summary>
    /// Change canvas UI, if needed.
    /// </summary>
    private void ChangeCanvasOpacity()
    {
        // If doesn't need to change UI visibility, exit
        if(!changeUI)
        {
            return;
        }

        // Change UI opacity over transition length seconds
        canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, (elapsedTime / transitionLength));
        elapsedTime += Time.deltaTime;

        // If UI opacity is the target opacity, reset elapsed time and set change UI controller to false
        if(canvasGroup.alpha == targetAlpha)
        {
            elapsedTime = 0f;
            changeUI = false;

            // If target opacity is 0, set disable store canvas
            if(targetAlpha == 0f)
            {
                storeCanvas.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Method for closing the store canvas.
    /// </summary>
    public void Close()
    {
        ObjectsManager.instance.StopHoverUI();
        SetUI(false);
        HUDManager.instance.SetHUD(true);
        InGameSaves.ChangeIsBusy();
        TutorialManager.instance.PauseTutorial();
    }

    // Method for Trigger Enter
    private void OnTriggerEnter(Collider other)
    {
        // Update store menu UI and set store canvas visibility to true
        StoreUI.instance.UpdateMenu();
        SetUI(true);
        HUDManager.instance.SetHUD(false);

        // Get player agent and stop animation
        myAgent = other.GetComponent<NavMeshAgent>();
        EndAnimationHandler(myAgent);        

        // Chang player busy state
        InGameSaves.ChangeIsBusy();

        // Set new level controller to false
        newItems = false;

        // Get tutorial
        TutorialManager.instance.GetTutorial("store");
    }

    /// <summary>
    /// Set UI visibility.
    /// </summary>
    /// <param name="isActive">New visibility.</param>
    public void SetUI(bool isActive)
    {
        // Set initial opacity
        initialAlpha = canvasGroup.alpha;

        // If the target is to display UI, set target opacity to 1
        if(isActive)
        {
            targetAlpha = 1f;
            storeCanvas.SetActive(true);
        }

        // If the target is to hide UI, set target opacity to 0
        else
        {
            targetAlpha = 0f;
        }

        // Set change UI controller to true
        changeUI = true;
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
    /// Add an item to sold items dictionary.
    /// </summary>
    /// <param name="name">Plant name, used as the dictionary key.</param>
    /// <param name="amount">Sold amount.</param>
    public void AddSoldItem(string name, int amount)
    {
        // Make sure the key is lower case
        name = name.ToLower();

        // If this item already exists in the dictionary, add the amount
        if(soldItems.ContainsKey(name))
        {
            soldItems[name] += amount;
        }

        // If the key is invalid, add it to dictionary
        else
        {
            soldItems.Add(name, amount);
        }
    }

    /// <summary>
    /// Check if player sold a certain amount of an item.
    /// </summary>
    /// <param name="name">Item name to check.</param>
    /// <param name="amount">Sold amount to check.</param>
    /// <returns>TRUE - If it did. FALSE - If it didn't.</returns>
    public bool HasSold(string name, int amount=1)
    {
        // Make sure the key is lower case
        name = name.ToLower();

        // If the item has not been sold, return false
        if(!soldItems.ContainsKey(name))
        {
            return false;
        }

        // If the amount is invalid, return true
        if(amount == -1)
        {
            return true;
        }

        // If the amount is valid, return if the amount of sold item is bigger
        // or equal to the given amount
        return soldItems[name] >= amount;
    }

    /// <summary>
    /// Clear all sold items.
    /// </summary>
    public void ClearSoldItems()
    {
        soldItems.Clear();
    }

    /// <summary>
    /// Check for new items on store.
    /// </summary>
    public void CheckNewItems()
    {
        int playerLevel = PlayerDataManager.instance.playerData.level - 1;
        foreach(Item item in storeItems)
        {
            if(playerLevel == item.levelRequired)
            {
                newItems = true;
                return;
            }
        }
    }

    /// <summary>
    /// Set the current selected item.
    /// </summary>
    /// <param name="selectedItem">New selected item.</param>
    public void SetSelectedItem(Item selectedItem)
    {
        this.selectedItem = selectedItem;
    }

    /// <summary>
    /// Get the current selected item.
    /// </summary>
    /// <returns></returns>
    public Item GetSelectedItem()
    {
        return selectedItem;
    }

    /// <summary>
    /// Set the current selected store slot.
    /// </summary>
    /// <param name="selectedSlot">New selected store slot.</param>
    public void SetSelectedStoreSlot(StoreSlot selectedSlot)
    {
        this.selectedSlot = selectedSlot;
    }

    /// <summary>
    /// Buy method.
    /// </summary>
    private void Buy()
    {
        // If there is no selected item, exit
        if(selectedItem == null)
        {
            return;
        }

        // Get the amount and buy it
        int amount = StoreUI.instance.GetSelectedAmount();
        PlayerDataManager.instance.Buy(selectedItem, amount);
    }

    /// <summary>
    /// Sell method.
    /// </summary>
    private void Sell()
    {
        // If there is no selected item, exit
        if(selectedItem == null)
        {
            return;
        }

        // Get amount
        int amount = StoreUI.instance.GetSelectedAmount();

        // Define multiplier
        float multiplier = 1f;

        // Calculate multiplier if the selected item is an item
        if(selectedItem.GetType() == typeof(Plant))
        {
            multiplier = 2f / (((Plant)selectedItem).seasons.IndexOf(TimeManager.instance.season) + 1);
        }
        
        // Sell item
        PlayerDataManager.instance.Sell(selectedItem, amount, multiplier);

        // Add the sold item to sold items dictionary
        AddSoldItem(selectedItem.name, amount);

        // Update slot
        selectedSlot.SellAmount(amount);
    }

    /// <summary>
    /// Buy or sell handler
    /// </summary>
    public void BuyOrSell()
    {
        // If the current tab is the buy menu, call buy method
        if(StoreUI.instance.isOnBuyMenu)
        {
            Buy();
        }

        // Else, call sell method
        else
        {
            Sell();
        }

        // Refresh money text
        StoreUI.instance.AmountSelectionMenu(false);
        StoreUI.instance.RefreshMoneyText();
    }
}
