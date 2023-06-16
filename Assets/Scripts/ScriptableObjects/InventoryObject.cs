using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    // Plants list
    public List<InventorySlotObject> plants = new List<InventorySlotObject>();


    /// <summary>
    /// Add a plant, with a given amount, to the inventory.
    /// Also, check if the plant have been harvested.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to add.</param>
    /// <param name="harvested">This plant was harvested?</param>
    public bool AddPlant(Plant plant, int amount, bool harvested)
    {
        // Loop all plants on inventory
        for(int i = 0; i < plants.Count; i++)
        {
            // If found the corresponding plant, add the amount to it's amount
            if(plants[i].plant == plant && plants[i].harvested == harvested)
            {
                plants[i].AddAmount(amount);
                return true;
            }
        }

        // If the plant does not exists, add it to the inventory
        plants.Add(new InventorySlotObject(plant, amount, harvested));
        return true;
    }


    /// <summary>
    /// Remove a certain amount of a given plant.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to remove.</param>
    /// <returns>TRUE - If successfully removed. FALSE - If failed to remove.</returns>
    public bool RemovePlant(Plant plant, int amount)
    {
        // Loop all plants
        for(int i = 0; i < plants.Count; i++)
        {
            // If found the corresponding plant, remove the given amount
            if(plants[i].plant == plant)
            {
                // Check if can remove the given amount
                bool canRemoveFromList = plants[i].RemoveAmount(amount);

                // If can, remove it
                if(canRemoveFromList)
                {
                    plants.RemoveAt(i);
                }

                return true;
            }
        }

        // Object doesn't exists in inventory
        return false;
    }
}

/// <summary>
/// Inventory slot class.
/// </summary>
[System.Serializable]
public class InventorySlotObject
{
    public Plant plant;
    public int amount;
    public bool harvested;

    /// <summary>
    /// Inventory Slot construct.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Plant amount.</param>
    /// <param name="harvested">Was this plant harvested?</param>
    public InventorySlotObject(Plant plant, int amount, bool harvested)
    {
        this.plant = plant;
        this.amount = amount;
        this.harvested = harvested;
    }

    /// <summary>
    /// Add slot amount.
    /// </summary>
    /// <param name="value">Amount to add.</param>
    public void AddAmount(int value)
    {
        amount += value;
    }

    /// <summary>
    /// Remove slot amount.
    /// </summary>
    /// <param name="value">amount to remove.</param>
    /// <returns>TRUE - If Successfully removed. FALSE - If failed to remove.</returns>
    public bool RemoveAmount(int value)
    {
        amount -= value;

        if(amount <= 0)
        {
            return true;
        }

        return false;
    }
}