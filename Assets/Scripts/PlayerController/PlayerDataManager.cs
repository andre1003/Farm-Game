using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;


public class PlayerDataManager : MonoBehaviour
{
    #region Singleton
    public static PlayerDataManager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerDataManager found!");
            return;
        }

        instance = this;
    }

    #endregion

    // Player data
    public PlayerData playerData;


    // Edit mode
    private bool editMode = false;


    // Start method
    void Start()
    {
        // Set edit mode to false
        editMode = false;
    }

    /// <summary>
    /// Method for buy an item.
    /// </summary>
    /// <param name="item">Item reference.</param>
    /// <param name="amount">Amount to buy.</param>
    public void Buy(Item item, int amount)
    {
        // If player does not has enought level, exit
        if(item.levelRequired > playerData.level)
        {
            return;
        }

        // If player does not have money to buy, exit
        if(item.buyValue > playerData.money)
        {
            return;
        }

        // Add to inventory
        if(item.GetType() == typeof(Plant))
        {
            Inventory.instance.Add((Plant)item, amount, false);
        }
        else
        {
            Inventory.instance.Add(item, amount);
        }
        
        playerData.money -= item.buyValue;
    }

    /// <summary>
    /// Sell an item.
    /// </summary>
    /// <param name="item">Item reference.</param>
    /// <param name="amount">Amount to sell.</param>
    /// <param name="multiplier">Price multiplier.</param>
    public void Sell(Item item, int amount, float multiplier = 1f)
    {
        bool hasRemoved;
        if(item.GetType() == typeof(Plant))
        {
            hasRemoved = Inventory.instance.Remove((Plant)item, amount, true);
        }
        else
        {
            hasRemoved = Inventory.instance.Remove(item, amount);
        }

        // If successfully removed from player inventory
        if(hasRemoved)
        {
            // Add money and update HUD
            playerData.money += item.baseSellValue * multiplier;
        }
    }

    /// <summary>
    /// Method for adding money.
    /// </summary>
    /// <param name="moneyToAdd">Money to add.</param>
    public void SetMoney(float moneyToAdd)
    {
        playerData.money += moneyToAdd;
    }

    /// <summary>
    /// Method for adding XP.
    /// </summary>
    /// <param name="xp">XP to add.</param>
    public void SetXp(int xp)
    {
        // Add XP
        playerData.xp += xp;

        // Check for level up
        if(playerData.xp >= playerData.nextLvlXp)
        {
            playerData.xp -= playerData.nextLvlXp;
            playerData.level++;
            playerData.nextLvlXp += 10;

            // Check for new items
            Store.instance.CheckNewItems();
        }

        // Don't let XP be negative
        else if(playerData.xp < 0)
        {
            playerData.xp = 0;
        }
    }

    /// <summary>
    /// Change edit mode.
    /// </summary>
    /// <param name="editMode">New edit mode value.</param>
    public void SetEditMode(bool editMode)
    {
        this.editMode = editMode;
    }

    /// <summary>
    /// Get edit mode value.
    /// </summary>
    /// <returns>Edit mode value.</returns>
    public bool GetEditMode()
    {
        return editMode;
    }
}
