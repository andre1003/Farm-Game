using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject {
    public int slots = 20;
    public List<InventorySlotObject> plants = new List<InventorySlotObject>();

    public bool AddPlant(Plant plant, int amount) {
        for(int i = 0; i < plants.Count; i++) {
            if(plants[i].plant == plant) {
                plants[i].AddAmount(amount);
                return true;
            }
        }

        if(slots > 0) {
            slots--;
            plants.Add(new InventorySlotObject(plant, amount));
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class InventorySlotObject {
    public Plant plant;
    public int amount;

    public InventorySlotObject(Plant plant, int amount) {
        this.plant = plant;
        this.amount = amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }
}