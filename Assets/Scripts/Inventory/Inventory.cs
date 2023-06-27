using UnityEngine;


public class Inventory : MonoBehaviour
{
    #region Singleton
    // On Item Change delegate
    public delegate void OnItemChange();
    public OnItemChange onItemChangeCallback;

    // Instance
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion


    // Inventory
    public InventoryObject inventory;


    /// <summary>
    /// Add a plant and amount to player's inventory.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to add.</param>
    /// <param name="harvested">This plant was harvested?</param>
    public void Add(Plant plant, int amount, bool harvested)
    {
        // Try to add plant to inventory
        inventory.AddPlant(plant, amount, harvested);

        // Call Item Change Callback if needed
        if(onItemChangeCallback != null)
        {
            onItemChangeCallback.Invoke();
        }
    }

    /// <summary>
    /// Add an item and amount to player's inventory.
    /// </summary>
    /// <param name="item">Item reference.</param>
    /// <param name="amount">Amount to add.</param>
    public void Add(Item item, int amount)
    {
        // Try to add item to inventory
        inventory.AddItem(item, amount);

        // Call Item Change Callback if needed
        if(onItemChangeCallback != null)
        {
            onItemChangeCallback.Invoke();
        }
    }

    /// <summary>
    /// Remove certain amount of a plant from inventory.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to remove.</param>
    /// <returns>TRUE - Successfully removed. FALSE - Failed to add.</returns>
    public bool Remove(Plant plant, int amount, bool harvested)
    {
        // Try to remove a given amount of a given plant
        bool hasRemoved = inventory.RemovePlant(plant, amount, harvested);

        // Call Item Change Callback if needed.
        if(onItemChangeCallback != null)
        {
            onItemChangeCallback.Invoke();
        }

        // Return if the plant have been successfully removed to inventory or not
        return hasRemoved;
    }
}
