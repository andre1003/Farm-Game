using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    // Plants list
    public List<InventorySlotObject> plants = new List<InventorySlotObject>();
    public List<InventorySlotObject> harvestedPlants = new List<InventorySlotObject>();

    // Items list
    public List<InventorySlotObject> items = new List<InventorySlotObject>();



    public void AddItem(Item item, int amount)
    {
        // Loop all items on inventory
        for(int i = 0; i < items.Count; i++)
        {
            // If found the corresponding item, add the amount to it's amount
            if((Plant)items[i].item == item)
            {
                items[i].AddAmount(amount);
                return;
            }
        }

        // If the item does not exists, add it to the inventory
        items.Add(new InventorySlotObject(item, amount));
    }


    /// <summary>
    /// Add a plant, with a given amount, to the inventory.
    /// Also, check if the plant have been harvested.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to add.</param>
    /// <param name="harvested">This plant was harvested?</param>
    public void AddPlant(Plant plant, int amount, bool harvested)
    {
        // Add to harvested plants list
        if(harvested)
        {
            AddPlantImplementation(harvestedPlants, plant, amount);
        }

        // Add to plants list
        else
        {
            AddPlantImplementation(plants, plant, amount);
        }
        
    }

    /// <summary>
    /// Implementation of adding a plant to inventory.
    /// </summary>
    /// <param name="list">List to add.</param>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to add.</param>
    private void AddPlantImplementation(List<InventorySlotObject> list, Plant plant, int amount)
    {
        // Loop all plants on inventory
        for(int i = 0; i < list.Count; i++)
        {
            // If found the corresponding plant, add the amount to it's amount
            if((Plant)list[i].item == plant)
            {
                list[i].AddAmount(amount);
                return;
            }
        }

        // If the plant does not exists, add it to the inventory
        list.Add(new InventorySlotObject(plant, amount));
    }


    /// <summary>
    /// Remove a certain amount of a given plant.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to remove.</param>
    /// <param name="harvested">This plant was harvested?</param>
    /// <returns>TRUE - If successfully removed. FALSE - If failed to remove.</returns>
    public bool RemovePlant(Plant plant, int amount, bool harvested)
    {
        if(harvested)
        {
            return RemovePlantImplementation(harvestedPlants, plant, amount);
        }

        else
        {
            return RemovePlantImplementation(plants, plant, amount);
        }
    }

    /// <summary>
    /// Implementation of removing a plant to inventory.
    /// </summary>
    /// <param name="list">List to remove.</param>
    /// <param name="plant"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    private bool RemovePlantImplementation(List<InventorySlotObject> list, Plant plant, int amount)
    {
        // Loop all plants
        for(int i = 0; i < list.Count; i++)
        {
            // If found the corresponding plant, remove the given amount
            if((Plant)list[i].item == plant)
            {
                // Check if can remove the given amount
                bool canRemoveFromList = list[i].RemoveAmount(amount);

                // If can, remove it
                if(canRemoveFromList)
                {
                    list.RemoveAt(i);
                }

                return true;
            }
        }

        // Object doesn't exists in inventory
        return false;
    }

    /// <summary>
    /// Check if the inventory has the plant with a given name.
    /// </summary>
    /// <param name="plantName">Name of the plant to search.</param>
    /// <returns>TRUE - If there is a plant. FALSE - If don't.</returns>
    public bool HasPlant(string plantName)
    {
        // Loop all inventory slots
        foreach(InventorySlotObject plant in plants)
        {
            // If this plant is the one we are looking for, return true
            if(plant.item.name.ToLower() == plantName.ToLower())
            {
                return true;
            }
        }

        // If didn't find the plant, return false
        return false;
    }
}




/// <summary>
/// Inventory slot class.
/// </summary>
[System.Serializable]
public class InventorySlotObject
{
    public Item item;
    public int amount;

    /// <summary>
    /// Inventory Slot construct.
    /// </summary>
    /// <param name="item">Item reference.</param>
    /// <param name="amount">Plant amount.</param>
    public InventorySlotObject(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
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