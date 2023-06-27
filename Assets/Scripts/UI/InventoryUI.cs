using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

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

    // Transition
    public float transitionLength = 0.25f;

    // Localization
    public TextMeshProUGUI moneyText;


    // Inventory references
    private Inventory inventory;
    private InventorySlot[] slots;

    // Tab controller
    private int tab = 0;

    // UI
    private CanvasGroup canvasGroup;
    private bool changeUI = false;

    // Transition
    private float initialAlpha;
    private float targetAlpha;
    private float elapsedTime = 0f;


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

        // Update UI
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        // If player press I ky and isBusy is false, change inventory visibility and update UI
        if(Input.GetKeyDown(KeyCode.I) && !InGameSaves.GetIsBusy())
        {
            HUDManager.instance.SetHUD(inventoryCanvas.activeSelf);
            SetUI(!inventoryCanvas.activeSelf);
            UpdateUI();
        }

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
        // Refresh money text
        moneyText.text = PlayerDataManager.instance.playerData.money.ToString("F2");

        // Get inventory slot, based on inventory tab
        List<InventorySlotObject> inventorySlots = GetSlots();

        // Loop slots
        for(int i = 0; i < slots.Length; i++)
        {
            // If its the correct inventory tab, add it to slot
            if(i < inventorySlots.Count)
            {
                slots[i].AddItem(inventorySlots[i].item, inventorySlots[i].amount);
            }

            // Else, clear slot
            else
            {
                slots[i].ClearSlot();
            } 
        }
    }

    private List<InventorySlotObject> GetSlots()
    {
        switch(tab)
        {
            case 0:
                return inventory.inventory.plants;

            case 1:
                return inventory.inventory.harvestedPlants;

            case 2:
                return inventory.inventory.items;

            default:
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
        UpdateUI();
    }

    /// <summary>
    /// Get current inventory tab.
    /// </summary>
    /// <returns>Current inventory tab.</returns>
    public int GetTab()
    {
        return this.tab;
    }
}
