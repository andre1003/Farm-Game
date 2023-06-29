using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    #region Singleton
    public static InventoryUI instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of InventoryUI found!");
            return;
        }

        instance = this;
    }

    #endregion

    // UI elements references
    public Transform itemsParent;
    public GameObject inventoryCanvas;
    public Button nextPageButton;
    public Button prevPageButton;
    public TextMeshProUGUI pageText;

    // Transition
    public float transitionLength = 0.25f;

    // Localization
    public TextMeshProUGUI moneyText;


    // Inventory references
    private Inventory inventory;
    private InventorySlot[] slots;

    // Tab controller
    private int tab = 0;
    private int page = 0;

    // UI
    private CanvasGroup canvasGroup;
    private bool changeUI = false;

    // Transition
    private float initialAlpha;
    private float targetAlpha;
    private float elapsedTime = 0f;

    // Input
    float horizontalAxis = 0f;


    // Start is called before the first frame update
    void Start()
    {
        // Get inventory and add UpdateUI method to On Item Change Callback delegate
        inventory = Inventory.instance;
        inventory.onItemChangeCallback += UpdateUI;

        // Get slots
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        // Get canvas group
        canvasGroup = inventoryCanvas.GetComponent<CanvasGroup>();

        // Disable next page button, if needed
        if(page * slots.Length >= GetSlots().Count)
        {
            nextPageButton.interactable = false;
        }

        // Disable previous page button
        prevPageButton.interactable = false;

        // Update UI
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
        ChangeCanvasOpacity();
    }

    /// <summary>
    /// Open or close inventory UI.
    /// </summary>
    public void OpenCloseInventory()
    {
        // If inventory will be closed, set mouse cursor to default
        if(inventoryCanvas.activeSelf)
        {
            ObjectsManager.instance.StopHoverUI();
        }

        HUDManager.instance.SetHUD(inventoryCanvas.activeSelf);
        SetUI(!inventoryCanvas.activeSelf);
        UpdateUI();
    }

    /// <summary>
    /// Input handler.
    /// </summary>
    private void InputHandler()
    {
        // If player press I ky and isBusy is false, change inventory visibility and update UI
        if(Input.GetKeyDown(KeyCode.I) && !InGameSaves.GetIsBusy())
        {
            OpenCloseInventory();
            return;
        }

        // If inventory canvas is not active, exit
        if(!inventoryCanvas.activeSelf)
        {
            return;
        }

        // If change tab button key have been pressed
        if(Input.GetButtonDown("ChangeTab"))
        {
            // If it is the las tab, set it to the first
            if(tab == 2)
            {
                SetTab(0);
            }

            // Else, set to next tab
            else
            {
                SetTab(tab + 1);
            }

            // Exit
            return;
        }

        // Get horizontal axis raw value
        float axis = Input.GetAxisRaw("Horizontal");

        // If horizontalAxis is equal axis, exit
        if(horizontalAxis == axis)
        {
            return;
        }

        // Set horizontalAxis value to axis
        horizontalAxis = axis;

        // If horizontal axis is negative (pressing left button) and the previous button
        // is interactable, get previous page
        if(horizontalAxis == -1f && prevPageButton.interactable)
        {
            PrevPage();
        }

        // If horizontal axis is positive (pressing right button) and the next button
        // is interactable, get next page
        else if(horizontalAxis == 1f && nextPageButton.interactable)
        {
            NextPage();
        }
    }
    
    /// <summary>
    /// Change canvas UI, if needed.
    /// </summary>
    private void ChangeCanvasOpacity()
    {
        // If doesn't need to change HUD visibility, exit
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
                MovementController.instance.ResumePlayer();
                inventoryCanvas.SetActive(false);
            }
        }
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
            inventoryCanvas.SetActive(true);
            MovementController.instance.StopPlayer();
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
    /// Method for update inventory UI.
    /// </summary>
    private void UpdateUI()
    {
        // Set page text
        pageText.text = (page + 1).ToString();

        // Get number of slots
        int slotsNumber = slots.Length;

        // Refresh money text
        moneyText.text = PlayerDataManager.instance.playerData.money.ToString("F2");

        // Get inventory slot, based on inventory tab
        List<InventorySlotObject> inventorySlots = GetSlots();

        // Loop slots
        for(int i = 0, j = page * slotsNumber; i < slotsNumber; i++, j++)
        {
            // If its the correct inventory tab, add it to slot
            if(j < inventorySlots.Count)
            {
                slots[i].AddItem(inventorySlots[j].item, inventorySlots[j].amount);
            }

            // Else, clear slot
            else
            {
                slots[i].ClearSlot();
            } 
        }
    }

    /// <summary>
    /// Get inventory slots.
    /// </summary>
    /// <returns>Inventory slots.</returns>
    private List<InventorySlotObject> GetSlots()
    {
        // Switch tab
        switch(tab)
        {
            case 0: // Plants
                return inventory.inventory.plants;

            case 1: // Harvested plants
                return inventory.inventory.harvestedPlants;

            case 2: // Items
                return inventory.inventory.items;

            default: // Null
                return null;
        }
    }

    /// <summary>
    /// Set inventory tab.
    /// </summary>
    /// <param name="tab">New inventory tab.</param>
    public void SetTab(int tab)
    {
        this.tab = tab;
        page = 0;
        prevPageButton.interactable = false;
        UpdateUI();

        // If there are no items on next page (after page increment), disable next page button
        if((page + 1) * slots.Length >= GetSlots().Count - 1)
        {
            nextPageButton.interactable = false;
        }

        // Else, enable next page button
        else
        {
            nextPageButton.interactable = true;
        }
    }

    /// <summary>
    /// Get current inventory tab.
    /// </summary>
    /// <returns>Current inventory tab.</returns>
    public int GetTab()
    {
        return this.tab;
    }

    /// <summary>
    /// Next inventory tab.
    /// </summary>
    public void NextPage()
    {
        // Increase page
        page++;

        // If there are no items on next page (after page increment), disable next page button
        if((page + 1) * slots.Length >= GetSlots().Count-1)
        {
            nextPageButton.interactable = false;
        }

        // Enable previous page button
        prevPageButton.interactable = true;

        // Update UI
        UpdateUI();
    }

    /// <summary>
    /// Previous inventory page.
    /// </summary>
    public void PrevPage()
    {
        // Decrease page
        page--; 

        // If it is the first page, disable previous page button
        if(page == 0)
        {
            prevPageButton.interactable = false;
        }

        // Enable next page button
        nextPageButton.interactable = true;

        // Update UI
        UpdateUI();
    }
}
