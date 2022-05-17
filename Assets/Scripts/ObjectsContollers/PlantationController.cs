using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantationController : MonoBehaviour {

    #region Singleton
    public static PlantationController instance;

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of Plantation Controller found!");
            return;
        }

        instance = this;
    }
    #endregion

    public Transform front;
    public List<Transform> spawns;

    public PlantationZones zones;

    private Plant planted;

    // Method for planting
    public void Plant(Plant plant) {
        // Plants only if player have in Inventory
        if(planted == null) {
            bool hasRemoved = Inventory.instance.Remove(plant);
            if(hasRemoved) {
                PlayerDataManager.instance.SetXp(plant.xp);
                
                MovementController.instance.SendTo(front.position, gameObject.transform);
                
                zones.SetPlant(transform.position, plant);
            }

            planted = plant;
            SpawnPlants(plant.plantPrefab);
        }
    }

    private void SpawnPlants(Transform plantPrefab) {
        foreach(var spawn in spawns) {
            Instantiate(plantPrefab, spawn.transform.position, Quaternion.identity);
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
