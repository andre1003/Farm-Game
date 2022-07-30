using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour {
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
    private int amount = -1;


    // Add a plant to a slot
    public void AddPlant(Plant plant) {
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

    // Clear the slot
    public void ClearSlot() {
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

    // Add a plant to sell
    public void AddPlantToSell(Plant plant, int amount) {
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

    // Buy a plant
    public void Buy() {
        if(plant != null)
            PlayerDataManager.instance.Buy(plant, 1);
    }

    // Sell a plant
    public void Sell() {
        if(plant != null) {
            // Calculate multiplier and sell plant
            float multiplier = 2f / (plant.seasons.IndexOf(TimeManager.instance.season) + 1);
            PlayerDataManager.instance.Sell(plant, 1, multiplier);

            // Decrease amount
            amount--;

            // If amount is less or equal to 0, clear the slot
            if(amount <= 0)
                ClearSlot();
            // Else, update the slot
            else
                AddPlantToSell(plant, amount);
            
        }
    }

    // Store handler
    public void StoreActionHandler() {
        if(StoreUI.instance.isOnBuyMenu)
            Buy();
        else
            Sell();
    }
}
