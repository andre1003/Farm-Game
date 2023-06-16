using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantationController : MonoBehaviour
{
    // PlantationZone variable
    public PlantationZones zones;

    #region Singleton

    public static PlantationController instance;

    private void Awake()
    {
        if(instance != null)
            return;

        instance = this;
    }
    #endregion

    // Transforms
    public Transform front;
    public List<Transform> spawns;

    // Time counter
    public int time = -1;

    // Initial planted day
    public int initialDay = 0;


    // Planted plant
    private Plant planted;

    private List<Transform> spawned;

    // Harvest and rot controller
    private bool isReadyToHarvest = false;
    private bool isRotten = false;

    // ID
    private int id = -1;


    void Update()
    {
        AddTime();
    }

    // Method for planting
    public void Plant(Plant plant)
    {
        // Plants only if player have in Inventory
        if(planted == null)
        {
            // Try to remove one plant from inventory
            bool hasRemoved = Inventory.instance.Remove(plant, 1);

            // If removed
            if(hasRemoved)
            {
                // Set XP
                PlayerDataManager.instance.SetXp(plant.xp);

                try
                {
                    // Move player to front spot to plant
                    MovementController.instance.SendTo(front.position, gameObject.transform);
                }
                catch { }

                // Add the platnt to zones variable
                zones.SetPlant(transform.position, plant);
            }

            // Set plant to planted plant and spawn the objects
            planted = plant;
            SpawnPlants(plant.plantPrefab);

            // Setup initial plant day
            if(initialDay == 0)
            {
                initialDay = TimeManager.instance.day;
                id = zones.AddTime(initialDay, id);
                Debug.Log("Planted in day: " + initialDay);
            }
        }
    }

    public void InitialSetup()
    {
        id = zones.AddTime(initialDay, id);
        time = 0;
    }

    public void AddTime()
    {
        if(planted != null)
        {
            // Add time
            time = TimeManager.instance.day - initialDay;
            //id = zones.AddTime(time, id);

            // If time is between grow and rot, player can harvest
            if(time >= planted.timeToGrow && time < planted.timeToRot)
            {
                Debug.Log("Ready to harvest!");
                isReadyToHarvest = true;
                isRotten = false;
            }
            // Else, if time is bigger or equal to time to rot, player can no longer harvest
            else if(time >= planted.timeToRot)
            {
                Debug.Log("Rotten!");
                isReadyToHarvest = false;
                isRotten = true;

                if(time >= planted.timeToRot + 10)
                {
                    // Set XP
                    PlayerDataManager.instance.SetXp(-planted.xp);

                    // Destroy plants
                    DestroyPlants();
                }
            }
        }
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void SetTimeAndId(int time, int id)
    {
        this.initialDay = time;
        this.id = id;
    }

    public bool HasPlant()
    {
        if(planted != null)
            return true;
        else
            return false;
    }

    public bool CanHarvest()
    {
        return isReadyToHarvest;
    }

    public bool IsRotten()
    {
        return isRotten;
    }

    public void Harvest()
    {
        Inventory.instance.Add(planted, Random.Range(planted.minHarvest, planted.maxHarvest), true);

        DestroyPlants();
    }

    private void SpawnPlants(Transform plantPrefab)
    {
        spawned = new List<Transform>();

        // For each spawn point, spawn a seed
        foreach(var spawn in spawns)
        {
            Transform newPlant = Instantiate(plantPrefab, spawn.transform.position, Quaternion.identity);
            newPlant.parent = transform;
            spawned.Add(newPlant);
        }
    }

    public void DestroyMe()
    {
        Debug.Log("DestroyMe called!");
        DestroyPlants();
        zones.RemoveTime(id);
    }

    public void DestroyPlants()
    {
        // Loop spawned plants to clear
        foreach(var spawn in spawned)
        {
            Destroy(spawn.gameObject);
        }

        // Clear time
        time = 0;
        id = zones.AddTime(0, id);
        initialDay = -1;

        // Clear planted plant
        zones.SetPlant(transform.position, null);
        planted = null;

        // Clear spawned list
        spawned.Clear();
    }

    // Test function
    private void OnTriggerEnter(Collider other)
    {
        if(planted != null)
            Debug.Log("Aqui está plantado " + planted.name);
        else
            Debug.Log("Área vazia!");
    }
}
