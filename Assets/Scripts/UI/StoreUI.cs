using System.Collections.Generic;
using TMPro;
using UnityEngine;


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

    // Amount selection
    public GameObject storeCanvas;
    public GameObject amountSelectionCanvas;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI buyOrSellButtonText;


    // Store
    private Store store;
    private StoreSlot[] slots;

    // Amount selection
    private int amount = 1;


    // Start is called before the first frame update
    void Start()
    {
        // Get store, slots and open buy menu
        store = Store.instance;
        slots = itemsParent.GetComponentsInChildren<StoreSlot>();

        amountText.text = amount.ToString();

        //BuyMenu();
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
        // Set tab controller
        isOnBuyMenu = true;

        // Display at inventory
        for(int i = 0; i < slots.Length; i++)
        {
            // If this index is valid for store plants inventory AND
            // player have enought level for buying it, add to slot.
            if(i < store.storePlants.Length &&
                store.storePlants[i].levelRequired <= PlayerDataManager.instance.playerData.level)
            {
                slots[i].AddPlant(store.storePlants[i]);
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
        // Set tab controller
        isOnBuyMenu = false;

        // Auxiliar variables
        List<Plant> plants = new List<Plant>();
        List<int> amounts = new List<int>();

        // Loop inventory to get harvested plants
        foreach(InventorySlotObject plant in Inventory.instance.inventory.plants)
        {
            // If the plant have been harvested, add to UI
            if(plant.harvested)
            {
                plants.Add(plant.plant);
                amounts.Add(plant.amount);
            }
        }

        // Display at inventory
        int length = slots.Length;
        for(int i = 0; i < length; i++)
        {
            // Add plant to sell slot
            if(i < plants.Count)
            {
                slots[i].AddPlantToSell(plants[i], amounts[i]);
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
    }

    /// <summary>
    /// Set amount selection menu.
    /// </summary>
    /// <param name="isActive">Active status of amount selection menu.</param>
    public void AmountSelectionMenu(bool isActive)
    {
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
        amountSelectionCanvas.SetActive(isActive);
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
            float buyValue = Store.instance.GetSelectedPlant().buyValue;
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
            foreach(var inventorySlot in Inventory.instance.inventory.plants)
            {
                // If this is the selected plant, get the amount and break the loop
                if(inventorySlot.plant == Store.instance.GetSelectedPlant())
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

        // Set amount text
        amountText.text = amount.ToString();
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

        // Set amount text
        amountText.text = amount.ToString();
    }

    /// <summary>
    /// Get the selected amount.
    /// </summary>
    /// <returns>Selected amount.</returns>
    public int GetSelectedAmount()
    {
        return amount;
    }
}
