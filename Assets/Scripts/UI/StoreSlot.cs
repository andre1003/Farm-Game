using UnityEngine;
using UnityEngine.UI;


public class StoreSlot : MonoBehaviour
{
    // Slot icon
    public Image icon;

    // Slot add-ons
    public Image levelImage;
    public Image costImage;

    // Slot texts
    public Text levelText;
    public Text costText;


    // Auxiliar variables
    private Plant plant;
    public int amount = -1;


    /// <summary>
    /// Add plant to slot.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    public void AddPlant(Plant plant)
    {
        // Plant setup
        this.plant = plant;
        icon.sprite = plant.icon;

        // Enable add-ons
        levelText.enabled = true;
        costText.enabled = true;

        // Texts setup
        levelText.text = plant.levelRequired.ToString();
        costText.text = plant.buyValue.ToString();

        // Enable icon, level and cost image
        icon.enabled = true;
        levelImage.enabled = true;
        costImage.enabled = true;
    }

    /// <summary>
    /// Clear the slot.
    /// </summary>
    public void ClearSlot()
    {
        // Clear plant
        plant = null;
        icon.sprite = null;

        // Disable images
        levelImage.enabled = false;
        costImage.enabled = false;

        // Disable texts
        levelText.enabled = false;
        costText.enabled = false;

        // Disable icon
        icon.enabled = false;
    }

    /// <summary>
    /// Add a plant to sell.
    /// </summary>
    /// <param name="plant">Plant reference.</param>
    /// <param name="amount">Amount.</param>
    public void AddPlantToSell(Plant plant, int amount)
    {
        // Plant setup
        this.plant = plant;
        icon.sprite = plant.icon;

        // Enable add-ons
        levelText.enabled = true;
        costText.enabled = true;

        // Display amount
        this.amount = amount;
        levelText.text = amount.ToString();

        // Calculate and display sell value
        float cost = plant.baseSellValue * (2f / (plant.seasons.IndexOf(TimeManager.instance.season) + 1));
        costText.text = cost.ToString();

        // Enable all images
        icon.enabled = true;
        levelImage.enabled = true;
        costImage.enabled = true;
    }

    /// <summary>
    /// Sell a certain amount of this plant.
    /// </summary>
    /// <param name="amount">Amount to sell.</param>
    public void SellAmount(int amount)
    {
        // Decrease amount
        this.amount -= amount;

        // If amount is less or equal to 0, clear the slot
        if(this.amount <= 0)
        {
            ClearSlot();
        }

        // Else, update the slot
        else
        {
            AddPlantToSell(plant, this.amount);
        }
    }

    /// <summary>
    /// Store handler.
    /// </summary>
    public void StoreActionHandler()
    {
        if(plant != null)
        {
            Store.instance.SetSelectedPlant(plant);
            Store.instance.SetSelectedStoreSlot(this);
            StoreUI.instance.AmountSelectionMenu(true);
        }
    }
}
