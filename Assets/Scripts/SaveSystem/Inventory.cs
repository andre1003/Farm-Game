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

    public InventoryObject inventory;

    // Method for adding plant and amount to inventory
    public bool Add(Plant plant, int amount) {
        bool added = inventory.AddPlant(plant, amount);
        if(added)
            if(onItemChangeCallback != null)
                onItemChangeCallback.Invoke();

        return added;            
    }

    // Method for removing a plant from inventory
    public bool Remove(Plant plant) {
        bool hasRemoved = inventory.RemovePlant(plant, 1);

        if(onItemChangeCallback != null)
            onItemChangeCallback.Invoke();

        return hasRemoved;
    }
}
