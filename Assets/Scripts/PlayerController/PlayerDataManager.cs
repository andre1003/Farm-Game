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
        //UpdateHUD();
    }

    #endregion

    // Player data
    public PlayerData playerData;

    // HUD text
    public Text levelText;
    public Text xpText;

    // Localization
    public LocalizeStringEvent moneyTextEvent;
    public LocalizeStringEvent levelTextEvent;


    // Edit mode
    private bool editMode = false;


    // Start method
    void Start()
    {
        // Set edit mode to false and update HUD
        editMode = false;
        UpdateHUD();
    }

    /// <summary>
    /// Method for buy a plant.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to buy.</param>
    public void Buy(Plant plant, int amount)
    {
        // If player does not has enought level, exit
        if(plant.levelRequired > playerData.level)
        {
            return;
        }

        // If player does not have money to buy, exit
        if(plant.buyValue > playerData.money)
        {
            return;
        }

        // Add to inventory
        Inventory.instance.Add(plant, amount, false);
        playerData.money -= plant.buyValue;
        UpdateHUD();
    }

    /// <summary>
    /// Sell a plant.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount to sell.</param>
    /// <param name="multiplier">Price multiplier.</param>
    public void Sell(Plant plant, int amount, float multiplier = 1f)
    {
        // If successfully removed from player inventory
        if(Inventory.instance.Remove(plant, amount, true))
        {
            // Add money and update HUD
            playerData.money += plant.baseSellValue * multiplier;
            UpdateHUD();
        }
    }

    /// <summary>
    /// Method for update money text.
    /// </summary>
    private void UpdateHUD()
    {
        // Refresh money and level string
        moneyTextEvent.RefreshString();
        levelTextEvent.RefreshString();

        // Update XP text
        xpText.text = playerData.xp + " / " + playerData.nextLvlXp;
    }

    /// <summary>
    /// Method for adding money.
    /// </summary>
    /// <param name="moneyToAdd">Money to add.</param>
    public void SetMoney(float moneyToAdd)
    {
        playerData.money += moneyToAdd;
        UpdateHUD();
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

        // Update HUD
        UpdateHUD();
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
