using System.Collections.Generic;
using UnityEngine;

public class PlantationController : MonoBehaviour {

    #region Singleton
    private float clockSeconds;
    public static PlantationController instance;

    private void Awake() {
        clockSeconds = InGameSaves.GetClockSeconds();

        if(instance != null) {
            Debug.LogWarning("More than one instance of Plantation Controller found!");
            return;
        }

        instance = this;
    }
    #endregion

    // Transforms
    public Transform front;
    public List<Transform> spawns;

    // PlantationZone variable
    public PlantationZones zones;

    // Time counter
    public int time = 0;


    // Planted plant
    private Plant planted;

    private List<Transform> spawned;

    // Harvest and rot controller
    private bool isReadyToHarvest = false;
    private bool isRotten = false;

    // ID
    private int id = -1;


    void Update() {
        clockSeconds -= Time.deltaTime;
        if(clockSeconds < 0) {
            clockSeconds = InGameSaves.GetClockSeconds();
            AddTime(1);
        }
    }

    // Method for planting
    public void Plant(Plant plant) {
        // Plants only if player have in Inventory
        if(planted == null) {
            // Try to remove one plant from inventory
            bool hasRemoved = Inventory.instance.Remove(plant);

            // If removed
            if(hasRemoved) {
                // Set XP
                PlayerDataManager.instance.SetXp(plant.xp);
                
                // Move player to front spot to plant
                MovementController.instance.SendTo(front.position, gameObject.transform);
                
                // Add the platnt to zones variable
                zones.SetPlant(transform.position, plant);
            }

            // Set plant to planted plant and spawn the objects
            planted = plant;
            SpawnPlants(plant.plantPrefab);
        }
    }

    public void AddTime(int time) {
        if(planted != null) {
            // Add time
            this.time += time;
            id = zones.AddTime(this.time, id);
            Debug.Log("Time added! Current time: " + this.time);

            // If time is between grow and rot, player can harvest
            if(this.time >= planted.timeToGrow && this.time < planted.timeToRot) {
                Debug.Log("Ready to harvest!");
                isReadyToHarvest = true;
                isRotten = false;
            }
            // Else, if time is bigger or equal to time to rot, player can no longer harvest
            else if(this.time >= planted.timeToRot) {
                Debug.Log("Rotten!");
                isReadyToHarvest = false;
                isRotten = true;

                if(this.time >= planted.timeToRot + 10) {
                    // Set XP
                    PlayerDataManager.instance.SetXp(-planted.xp);

                    // Destroy plants
                    DestroyPlants();
                }
            }
        }
    }

    public void SetTimeAndId(int time, int id) {
        this.time = time;
        this.id = id;
    }

    public bool HasPlant() {
        if(planted != null)
            return true;
        else
            return false;
    }

    public bool CanHarvest() {
        return isReadyToHarvest;
    }

    public bool IsRotten() {
        return isRotten;
    }

    public void Harvest() {
        Inventory.instance.Add(planted, 1, true);

        DestroyPlants();
    }

    private void SpawnPlants(Transform plantPrefab) {
        spawned = new List<Transform>();

        // For each spawn point, spawn a seed
        foreach(var spawn in spawns) {
            Transform newPlant = Instantiate(plantPrefab, spawn.transform.position, Quaternion.identity);
            spawned.Add(newPlant);
        }
    }

    public void DestroyPlants() {
        // Loop spawned plants to clear
        foreach(var spawn in spawned) {
            Destroy(spawn.gameObject);
        }

        // Clear time
        time = 0;
        id = zones.AddTime(time, id);

        // Clear planted plant
        planted = null;
        zones.RemovePlant(id);

        // Clear spawned list
        spawned.Clear();
    }

    // Test function
    private void OnTriggerEnter(Collider other) {
        if(planted != null)
            Debug.Log("Aqui está plantado " + planted.name);
        else
            Debug.Log("Área vazia!");
    }
}
