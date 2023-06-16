using UnityEngine;


public class Inventory : MonoBehaviour {
    #region Singleton
    // On Item Change delegate
    public delegate void OnItemChange();
    public OnItemChange onItemChangeCallback;

    // Instance
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


    /// <summary>
    /// Add a plant and amount to player's inventory.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to add.</param>
    /// <param name="harvested">This plant was harvested?</param>
    /// <returns>TRUE - Successfully added. FALSE - Failed to add.</returns>
    public bool Add(Plant plant, int amount, bool harvested) {
        // Try to add plant to inventory
        bool added = inventory.AddPlant(plant, amount, harvested);

        // If plant successfully added
        if(added)
        {
            // Call Item Change Callback if needed
            if(onItemChangeCallback != null)
            {
                onItemChangeCallback.Invoke();
            }
        }

        // Return if the plant have been successfully added to inventory or not
        return added;            
    }


    /// <summary>
    /// Remove certain amount of a plant from inventory.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to remove.</param>
    /// <returns>TRUE - Successfully removed. FALSE - Failed to add.</returns>
    public bool Remove(Plant plant, int amount) {
        // Try to remove a given amount of a given plant
        bool hasRemoved = inventory.RemovePlant(plant, amount);

        // Call Item Change Callback if needed.
        if(onItemChangeCallback != null)
        {
            onItemChangeCallback.Invoke();
        }

        // Return if the plant have been successfully removed to inventory or not
        return hasRemoved;
    }
}
