using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    public Image icon;
    public Text text;
    
    private Plant plant;

    public void AddPlant(Plant plant, int amount) {
        this.plant = plant;
        icon.sprite = plant.icon;
        text.text = amount.ToString();
        icon.enabled = true;
    }

    public void ClearSlot() {
        plant = null;
        icon.sprite = null;
        text.text = "0";
        icon.enabled = false;
    }

    public void Plant() {
        GameObject plantationZone = InGameSaves.GetPlantationZone();
        
        if(plant != null && plantationZone != null) {
            plantationZone.GetComponent<PlantationController>().Plant(plant);
        }
    }
}
