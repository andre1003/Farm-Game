using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    public Image icon;
    
    private Plant plant;

    public void AddPlant(Plant plant) {
        this.plant = plant;
        icon.sprite = plant.icon;
        icon.enabled = true;
    }

    public void ClearSlot() {
        plant = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void Plant() {
        GameObject plantationZone = InGameSaves.GetPlantationZone();
        
        if(plant != null && plantationZone != null) {
            plantationZone.GetComponent<PlantationController>().Plant(plant);
        }
    }
}
