using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    // Particles
    public Transform grownParticleSystem;

    // Time counter
    public int time = -1;

    // Initial planted day
    public int initialDay = 0;
    public int initialHour = 0;


    // Plant
    private Plant planted;
    private List<Transform> spawned;
    private float interval;

    // Harvest and rot controller
    private bool isReadyToHarvest = false;
    private bool isRotten = false;

    // ID
    private int id = -1;


    // Update method
    void Update()
    {
        AddTime();
    }

    /// <summary>
    /// Method for planting.
    /// </summary>
    /// <param name="plant">Plant to be planted.</param>
    public void Plant(Plant plant)
    {
        // Plants only if player have in Inventory
        if(planted == null)
        {
            // Try to remove one plant from inventory
            bool hasRemoved = Inventory.instance.Remove(plant, 1, false);

            // If removed
            if(hasRemoved)
            {
                // Set XP
                PlayerDataManager.instance.SetXp(plant.xp);

                // Add the platnt to zones variable
                zones.SetPlant(transform.position, plant);
            }

            // Set plant to planted plant and spawn the objects
            planted = plant;
            interval = MovementController.instance.GetPlantLength() / spawns.Count;
            SpawnPlants(plant.smallPlantPrefab, 0);

            // Setup initial plant day
            if(initialDay == 0)
            {
                initialDay = TimeManager.instance.day;
                initialHour = TimeManager.instance.hour;
                id = zones.AddTime(initialDay, initialHour, id);
            }
        }
    }

    /// <summary>
    /// Initial plantation setup.
    /// </summary>
    public void InitialSetup()
    {
        id = zones.AddTime(initialDay, initialHour, id);
        time = 0;
    }

    /// <summary>
    /// Add time for this plantation zone.
    /// </summary>
    public void AddTime()
    {
        if(planted != null)
        {
            // Add time. The formula is:
            // ((Current Day - Planting Day) * 24) + (Current Hour - Planting Hour)
            time = ((TimeManager.instance.day - initialDay) * 24) + (TimeManager.instance.hour - initialHour);

            // If time is between grow and rot, player can harvest
            if(time >= planted.timeToGrow && time < planted.timeToRot && !isReadyToHarvest)
            {
                isReadyToHarvest = true;
                isRotten = false;
                ChangePlantPrefab(2);
                PlayParticles();
            }
            // Else, if time is bigger or equal to time to rot, player can no longer harvest
            else if(time >= planted.timeToRot && isReadyToHarvest)
            {
                isReadyToHarvest = false;
                isRotten = true;
                ChangePlantPrefab(3);

                // If have passe timeToRot + 10, remove XP and destroy the plants
                if(time >= planted.timeToRot + 10)
                {
                    PlayerDataManager.instance.SetXp(-planted.xp);
                    DestroyPlants();
                }
            }
        }
    }

    private void PlayParticles()
    {
        if(grownParticleSystem != null)
        {
            Vector3 position = transform.position + new Vector3(0f, 2f, 0f);
            Instantiate(grownParticleSystem, position, Quaternion.identity, transform);
        }
    }

    /// <summary>
    /// Change plant prefab based on status.
    /// <para>1. Medium;</para>
    /// <para>2. Grown;</para>
    /// <para>3. Rotten;</para>
    /// <para>Default. Small;</para>
    /// </summary>
    /// <param name="status">Plant status.</param>
    private void ChangePlantPrefab(int status)
    {
        // Get corresponding prefab
        Transform prefab;
        switch(status)
        {
            case 1: // Medium
                prefab = planted.mediumPlantPrefab;
                break;

            case 2: // Grown
                prefab = planted.grownPlantPrefab;
                break;

            case 3: // Rotten
                prefab = planted.rottenPlantPrefab;
                break;

            default: // Small
                prefab = planted.smallPlantPrefab;
                break;
        }

        // Change plants prefab
        int length = spawned.Count;

        // Loop spawned plants to clear
        foreach(var spawn in spawned)
        {
            Destroy(spawn.gameObject);
        }

        spawned.Clear();

        for(int i = 0; i < length; i++)
        {
            Spawn(prefab, i);
        }
    }

    public void LoadPlants(Plant plant)
    {
        planted = plant;

        if(planted == null)
        {
            return;
        }

        Transform prefab = plant.smallPlantPrefab;

        // Add time. The formula is:
        // ((Current Day - Planting Day) * 24) + (Current Hour - Planting Hour)
        time = ((TimeManager.instance.day - initialDay) * 24) + (TimeManager.instance.hour - initialHour);

        // If time is between grow and rot, player can harvest
        if(time >= planted.timeToGrow && time < planted.timeToRot)
        {
            isReadyToHarvest = true;
            isRotten = false;
            prefab = plant.grownPlantPrefab;
            PlayParticles();
        }
        // Else, if time is bigger or equal to time to rot, player can no longer harvest
        else if(time >= planted.timeToRot)
        {
            isReadyToHarvest = false;
            isRotten = true;
            prefab = plant.rottenPlantPrefab;
        }

        spawned = new List<Transform>();
        for(int i = 0; i < spawns.Count; i++)
        {
            Spawn(prefab, i);
        }
    }


    /// <summary>
    /// Set plantation zone ID.
    /// </summary>
    /// <param name="id">New ID value.</param>
    public void SetId(int id)
    {
        this.id = id;
    }

    /// <summary>
    /// Set time and ID of this plantation zone.
    /// </summary>
    /// <param name="initialDay">Initial day value.</param>
    /// <param name="initialHour">Initial hour value.</param>
    /// <param name="id">ID value.</param>
    public void SetTimeAndId(int initialDay, int initialHour, int id)
    {
        this.initialDay = initialDay;
        this.initialHour = initialHour;
        this.id = id;
    }

    /// <summary>
    /// Check if this plantation zone have a plant.
    /// </summary>
    public bool HasPlant()
    {
        // If there is a plant, return true
        if(planted != null)
        {
            return true;
        }

        // If there is no plant, return false
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Check if can harvest.
    /// </summary>
    public bool CanHarvest()
    {
        return isReadyToHarvest;
    }

    /// <summary>
    /// Check if the plant is rotten.
    /// </summary>
    public bool IsRotten()
    {
        return isRotten;
    }

    /// <summary>
    /// Harvest the plant.
    /// </summary>
    public void Harvest()
    {
        Inventory.instance.Add(planted, UnityEngine.Random.Range(planted.minHarvest, planted.maxHarvest), true);
        DestroyPlants();
    }

    /// <summary>
    /// Wait for seconds before spawn next plant.
    /// </summary>
    /// <param name="seconds">Seconds to wait.</param>
    /// <param name="plantPrefab">Plants prefab reference.</param>
    private IEnumerator WaitForSpawnPlants(float seconds, Transform plantPrefab, int index)
    {
        yield return new WaitForSeconds(seconds);
        SpawnPlants(plantPrefab, index + 1);
    }

    /// <summary>
    /// Spawn plants at the corresponding location.
    /// </summary>
    /// <param name="plantPrefab">Plant prefab.</param>
    /// <param name="index">List index.</param>
    private void SpawnPlants(Transform plantPrefab, int index)
    {
        // If spawned list is null, initialize it
        if(spawned == null)
        {
            spawned = new List<Transform>();
        }

        // If spawned list count is equal spawns count, exit
        if(spawned.Count == spawns.Count)
        {
            return;
        }

        // Spawn plant at the corresponding location and wait for spawn next plant
        Spawn(plantPrefab, index);
        StartCoroutine(WaitForSpawnPlants(interval, plantPrefab, index));
    }

    /// <summary>
    /// Spawn a plant at a location.
    /// </summary>
    /// <param name="plantPrefab">Plant prefab.</param>
    /// <param name="index">List index.</param>
    private void Spawn(Transform plantPrefab, int index)
    {
        Transform newPlant = Instantiate(plantPrefab, spawns[index].transform.position, Quaternion.identity);
        newPlant.parent = transform;
        spawned.Add(newPlant);
    }

    /// <summary>
    /// Custom destroy method.
    /// </summary>
    public void DestroyMe()
    {
        //Debug.Log("DestroyMe called!");
        DestroyPlants();
        zones.RemoveTime(id);
    }

    /// <summary>
    /// Destroy all plants of this plantation zone.
    /// </summary>
    public void DestroyPlants()
    {
        // Loop spawned plants to clear
        foreach(var spawn in spawned)
        {
            Destroy(spawn.gameObject);
        }

        // Clear time
        time = 0;
        id = zones.AddTime(0, 0, id);
        initialDay = -1;

        // Clear planted plant
        zones.SetPlant(transform.position, null);
        planted = null;

        // Clear spawned list
        spawned.Clear();
    }

    //// Test function
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(planted != null)
    //        Debug.Log("Aqui est� plantado " + planted.name);
    //    else
    //        Debug.Log("�rea vazia!");
    //}
}
