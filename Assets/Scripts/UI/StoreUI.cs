using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour {
    #region Singleton
    public static StoreUI instance;

    void Awake() {
        if(instance != null)
            return;

        instance = this;
    }
    #endregion

    public Transform itemsParent;
    public bool isOnBuyMenu = true;

    private Store store;
    private StoreSlot[] slots;

    // Start is called before the first frame update
    void Start() {
        store = Store.instance;
        slots = itemsParent.GetComponentsInChildren<StoreSlot>();
        BuyMenu();
    }

    public void ClearUI() {
        for(int i = 0; i < slots.Length; i++)
            slots[i].ClearSlot();
    }

    public void BuyMenu() {
        // Set tab controller
        isOnBuyMenu = true;

        // Display at inventory
        for(int i = 0; i < slots.Length; i++) {
            if(i < store.storePlants.Length)
                slots[i].AddPlant(store.storePlants[i]);
            else
                slots[i].ClearSlot();
        }
    }

    public void SellMenu() {
        // Set tab controller
        isOnBuyMenu = false;

        // Auxiliar variables
        List<Plant> plants = new List<Plant>();
        List<int> amounts = new List<int>();

        // Loop inventory to get harvested plants
        foreach(InventorySlotObject plant in Inventory.instance.inventory.plants) {
            if(plant.harvested) {
                plants.Add(plant.plant);
                amounts.Add(plant.amount);
            }
        }

        // Display at inventory
        int length = slots.Length;
        for(int i = 0; i < length; i++) {
            if(i < plants.Count)
                slots[i].AddPlantToSell(plants[i], amounts[i]);
            else
                slots[i].ClearSlot();
        }
    }
}
