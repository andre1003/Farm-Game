using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantationController : MonoBehaviour {

    #region Singleton
    public static PlantationController instance;

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

    private Plant planted;

    // Method for planting
    public void Plant(Plant plant) {
        // Plants only if player have in Inventory
        if(Inventory.instance.plants.Contains(plant) && planted == null) {
            Inventory.instance.Remove(plant);
            PlayerDataManager.instance.SetXp(plant.xp);
            planted = plant;
            Debug.Log(plant.name + " plantado com sucesso");
        }
    }

    // Test function
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Aqui está plantado " + planted.name);
    }
}
