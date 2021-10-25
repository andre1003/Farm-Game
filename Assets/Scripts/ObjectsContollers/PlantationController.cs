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

    public Transform front;

    private Plant planted;

    // Method for planting
    public void Plant(Plant plant) {
        // Plants only if player have in Inventory
        if(planted == null) {
            bool hasRemoved = Inventory.instance.Remove(plant);
            if(hasRemoved) {
                PlayerDataManager.instance.SetXp(plant.xp);
                planted = plant;
                MovementController.instance.SendTo(front.position, gameObject.transform);
                Debug.Log(plant.name + " plantado com sucesso");
            }
            else {
                Debug.Log("Você não possui nenhum(a) " + plant.name);
            }
        }
    }

    // Test function
    private void OnTriggerEnter(Collider other) {
        if(planted != null)
            Debug.Log("Aqui está plantado " + planted.name);
        else
            Debug.Log("Área vazia!");
    }
}
