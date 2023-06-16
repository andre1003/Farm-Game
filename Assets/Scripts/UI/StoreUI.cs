using System.Collections.Generic;
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


    // Store
    private Store store;
    private StoreSlot[] slots;


    // Start is called before the first frame update
    void Start()
    {
        // Get store, slots and open buy menu
        store = Store.instance;
        slots = itemsParent.GetComponentsInChildren<StoreSlot>();
        BuyMenu();
    }

    /// <summary>
    /// Clear all slots UI.
    /// </summary>
    public void ClearUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
        }

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
            // Add a plant to slot
            if(i < store.storePlants.Length)
            {
                slots[i].AddPlant(store.storePlants[i]);
            }

            // Clear slot
            else
            {
                slots[i].ClearSlot();
            }
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
}
