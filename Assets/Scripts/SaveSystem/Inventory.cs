using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    #region Singleton

    public delegate void OnItemChange();
    public OnItemChange onItemChangeCallback;

    public static Inventory instance;

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

    //public List<Plant> plants = new List<Plant>();
    //public int slots = 10;
    public InventoryObject inventory;

    // Method for adding plant and amount to inventory
    public bool Add(Plant plant, int amount) {
        bool added = inventory.AddPlant(plant, amount);
        if(added)
            if(onItemChangeCallback != null)
                onItemChangeCallback.Invoke();

        return added;            
    }

    public void Remove(Plant plant) {
        //inventory.RemovePlant(plant, 1);

        if(onItemChangeCallback != null)
            onItemChangeCallback.Invoke();
    }
}
