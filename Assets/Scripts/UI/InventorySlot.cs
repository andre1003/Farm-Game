using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    public Image icon;
    public Image amountImage;

    public Text text;
    
    private Plant plant;

    public void AddPlant(Plant plant, int amount) {
        this.plant = plant;

        icon.sprite = plant.icon;
        amountImage.enabled = true;
        icon.enabled = true;

        text.enabled = true;
        text.text = amount.ToString();
    }

    public void ClearSlot() {
        plant = null;

        icon.sprite = null;
        icon.enabled = false;
        amountImage.enabled = false;
        
        text.enabled = false;
    }

    public void Plant() {
        GameObject plantationZone = InGameSaves.GetPlantationZone();
        
        if(plant != null && plantationZone != null) {
            plantationZone.GetComponent<PlantationController>().Plant(plant);
        }
    }
}
