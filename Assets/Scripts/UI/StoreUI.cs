using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    #region Singleton
    public static StoreUI instance;

    void Awake()
    {
        if(instance != null)
            return;

        instance = this;
    }
    #endregion

    // Items parent
    public Transform itemsParent;

    // Buy menu controller
    public bool isOnBuyMenu = true;
    public bool filterPlants = true;

    // Amount selection
    public GameObject storeCanvas;
    public GameObject amountSelectionCanvas;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI totalText;
    public TextMeshProUGUI buyOrSellButtonText;

    // Money
    public TextMeshProUGUI moneyText;

    // UI
    public Button nextPageButton;
    public Button prevPageButton;
    public TextMeshProUGUI pageText;


    // Store
    private Store store;
    private StoreSlot[] slots;

    // Amount selection
    private int amount = 1;

    // Page
    private int page = 0;

    // Input
    float horizontalAxis = 0f;


    // Start is called before the first frame update
    void Start()
    {
        // Get store, slots and open buy menu
        store = Store.instance;
        slots = itemsParent.GetComponentsInChildren<StoreSlot>();

        // Set amount text
        amountText.text = amount.ToString();

        // Define length variable
        int length;

        // If it is on buy menu, get store plants length
        if(isOnBuyMenu)
        {
            length = store.storeItems.Length;
        }

        // If not, get harversted plant lentgth
        else
        {
            length = Inventory.instance.inventory.harvestedPlants.Count;
        }

        // If there are no items on next page (after page increment), disable next page button
        if((page + 1) * slots.Length >= length - 1)
        {
            nextPageButton.interactable = false;
        }

        // Enable previous page button
        prevPageButton.interactable = false;
    }

    // Update method
    void Update()
    {
        InputHandler();
    }

    /// <summary>
    /// Input handler.
    /// </summary>
    private void InputHandler()
    {
        // If store canvas is not active, exit
        if(!storeCanvas.activeSelf)
        {
            return;
        }

        // If change tab button key have been pressed
        if(Input.GetButtonDown("ChangeTab"))
        {
            // Reset page and previous page button active status
            page = 0;
            prevPageButton.interactable = false;

            // If it is not on buy menu
            if(!isOnBuyMenu)
            {
                nextPageButton.interactable = !((page + 1) * slots.Length >= store.storeItems.Length);
            }

            // If it is on buy menu
            else
            {
                nextPageButton.interactable = !((page + 1) * slots.Length >= Inventory.instance.inventory.harvestedPlants.Count);
            }

            // Change isOnBuyMenu value
            isOnBuyMenu = !isOnBuyMenu;

            // Update menu
            UpdateMenu();

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
    /// Clear all slots UI.
    /// </summary>
    public void ClearUI()
    {
        // Loop all slots and clear them all
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
        }

        // Disable amount selection canvas
        amountSelectionCanvas.SetActive(false);
    }

    /// <summary>
    /// Open buy menu.
    /// </summary>
    public void BuyMenu()
    {
        // If menu changed, update page
        if(!isOnBuyMenu)
        {
            page = 0;
            nextPageButton.interactable = !((page + 1) * slots.Length >= store.storeItems.Length);
            prevPageButton.interactable = false;
        }

        // Update page text
        pageText.text = (page + 1).ToString();

        // Set tab controller
        isOnBuyMenu = true;

        // Get number of slots and define items list
        int slotsNumber = slots.Length;
        List<Item> items = new List<Item>();

        // Loop store items, starting at the current page
        for(int i = page * slotsNumber; i < store.storeItems.Length; i++)
        {
            // Get item
            Item item = store.storeItems[i];

            // If item required level is less or equal player's level
            // AND Item type is equal to filter type
            // THEN Add item to items list
            if(item.levelRequired <= PlayerDataManager.instance.playerData.level &&
                (item.GetType() == typeof(Plant)) == filterPlants)
            {
                items.Add(item);
            }
        }

        // Loop slots
        for(int i = 0; i < slotsNumber; i++)
        {
            // If 'i' is a valid index for items list, add item to slot and continue
            if(i < items.Count)
            {
                slots[i].AddItem(items[i]);
                continue;
            }

            // Clear slot
            slots[i].ClearSlot();
        }
    }

    /// <summary>
    /// Open sell menu.
    /// </summary>
    public void SellMenu()
    {
        // If menu changed, update page
        if(isOnBuyMenu)
        {
            page = 0;
            nextPageButton.interactable = !((page + 1) * slots.Length >= Inventory.instance.inventory.harvestedPlants.Count);
            prevPageButton.interactable = false;
        }

        // Update page text
        pageText.text = (page + 1).ToString();

        // Set tab controller
        isOnBuyMenu = false;

        // Auxiliar variables
        List<Plant> plants = new List<Plant>();
        List<int> amounts = new List<int>();

        // Loop inventory to get harvested plants
        foreach(InventorySlotObject plant in Inventory.instance.inventory.harvestedPlants)
        {
            // If the plant have been harvested, add to UI
            plants.Add((Plant)plant.item);
            amounts.Add(plant.amount);
        }

        // Display at inventory
        int length = slots.Length;
        for(int i = 0, j = page * length; i < length; i++, j++)
        {
            // Add plant to sell slot
            if(j < plants.Count)
            {
                slots[i].AddItemToSell(plants[j], amounts[j]);
            }

            // Clear slot
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    /// <summary>
    /// Update menu tab.
    /// </summary>
    public void UpdateMenu()
    {
        // Buy tab
        if(isOnBuyMenu)
        {
            BuyMenu();
        }

        // Sell tab
        else
        {
            SellMenu();
        }

        // Refresh money text
        RefreshMoneyText();
    }

    /// <summary>
    /// Set amount selection menu.
    /// </summary>
    /// <param name="isActive">Active status of amount selection menu.</param>
    public void AmountSelectionMenu(bool isActive)
    {
        if(!isActive)
        {
            amountSelectionCanvas.SetActive(false);
            return;
        }

        // Set amount
        amount = 1;
        amountText.text = amount.ToString();

        // Define localization table and key
        string tableName = "Store Text";
        string key;

        // If it is the buy tab, get the buy localized string
        if(isOnBuyMenu)
        {
            key = "Buy Button";
        }

        // Else, get the sell localized string
        else
        {
            key = "Sell Button";
        }

        // Localize the string
        StringLocalizer.instance.Localize(tableName, key, buyOrSellButtonText);

        // Activate amount selection menu
        amountSelectionCanvas.SetActive(true);
        UpdateTotal();
    }

    /// <summary>
    /// Increase amount.
    /// </summary>
    public void IncreaseAmount()
    {
        // Increase amount
        amount++;

        // If it is the buying menu
        if(isOnBuyMenu)
        {
            // Get plant buy value and player money
            float buyValue = Store.instance.GetSelectedItem().buyValue;
            float playerMoney = PlayerDataManager.instance.playerData.money;

            // If the total buy value is bigger than player money, remove one from amount
            if(amount * buyValue > playerMoney)
            {
                amount--;
            }
        }

        // If it is the selling menu
        else
        {
            // Define amount on inventory
            int amountOnInventory = 0;

            // Find the selected plant
            foreach(var inventorySlot in Inventory.instance.inventory.harvestedPlants)
            {
                // If this is the selected plant, get the amount and break the loop
                if((Plant)inventorySlot.item == Store.instance.GetSelectedItem())
                {
                    amountOnInventory = inventorySlot.amount;
                    break;
                }
            }

            // If the current amount is bigger than amount on inventory, set it to amount on inventory
            if(amount > amountOnInventory)
            {
                amount = amountOnInventory;
            }
        }

        // Set amount and total text
        amountText.text = amount.ToString();
        UpdateTotal();
    }

    /// <summary>
    /// Decrease amount.
    /// </summary>
    public void DecreaseAmount()
    {
        // Decrease amount
        amount--;

        // If amount is lesser or equal to 0, set te amount to 1
        if(amount <= 0)
        {
            amount = 1;
        }

        // Set amount and text
        amountText.text = amount.ToString();
        UpdateTotal();
    }

    /// <summary>
    /// Update total text.
    /// </summary>
    private void UpdateTotal()
    {
        // If there is no selected plant, exit
        Item selectedItem = Store.instance.GetSelectedItem();
        if(selectedItem == null)
        {
            return;
        }

        // Set total value
        float total = selectedItem.buyValue * amount;
        totalText.text = total.ToString("F2");

        // Refresh money text
        RefreshMoneyText();
    }

    /// <summary>
    /// Get the selected amount.
    /// </summary>
    /// <returns>Selected amount.</returns>
    public int GetSelectedAmount()
    {
        return amount;
    }

    /// <summary>
    /// Refresh money text.
    /// </summary>
    public void RefreshMoneyText()
    {
        moneyText.text = PlayerDataManager.instance.playerData.money.ToString("F2");
    }

    /// <summary>
    /// Next store page.
    /// </summary>
    public void NextPage()
    {
        // Increase page
        page++;

        // Define length variable
        int length;

        // If it is on buy menu, get store plants length
        if(isOnBuyMenu)
        {
            length = store.storeItems.Length;
        }

        // If not, get harversted plant lentgth
        else
        {
            length = Inventory.instance.inventory.harvestedPlants.Count;
        }

        // If there are no items on next page (after page increment), disable next page button
        if((page + 1) * slots.Length >= length - 1)
        {
            nextPageButton.interactable = false;
        }

        // Enable previous page button
        prevPageButton.interactable = true;

        // Update menu
        UpdateMenu();
    }

    /// <summary>
    /// Previous store page.
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

        // Update menu
        UpdateMenu();
    }

    /// <summary>
    /// Set new value for plants filter controller.
    /// </summary>
    /// <param name="filterPlants">New value for plants filter controller.</param>
    public void SetFilterPlants(bool filterPlants)
    {
        this.filterPlants = filterPlants;
        UpdateMenu();
    }
}
