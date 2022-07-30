using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    public Image icon;
    public Image amountImage;

    public Text text;
    
    private Plant plant;

    /// <summary>
    /// Add a certain type of plant to the inventory slot.
    /// </summary>
    /// <param name="plant">Plant to be added to inventory slot.</param>
    /// <param name="amount">Amount of plant to be added.</param>
    public void AddPlant(Plant plant, int amount) {
        this.plant = plant;

        icon.sprite = plant.icon;
        amountImage.enabled = true;
        icon.enabled = true;

        text.enabled = true;
        text.text = amount.ToString();
    }

    /// <summary>
    /// Clear the slot
    /// </summary>
    public void ClearSlot() {
        plant = null;

        icon.sprite = null;
        icon.enabled = false;
        amountImage.enabled = false;
        
        text.enabled = false;
    }

    /// <summary>
    /// Plant the selected item in inventory at the selected plantation zone
    /// </summary>
    public void Plant() {
        GameObject plantationZone = InGameSaves.GetPlantationZone();
        if(plant != null && plantationZone != null && !InventoryUI.instance.GetHarvested()) {
            InventoryUI.instance.inventoryCanvas.SetActive(false);
            plantationZone.GetComponent<PlantationController>().Plant(plant);
        }
    }
}
