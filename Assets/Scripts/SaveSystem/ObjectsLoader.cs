using System.Collections.Generic;
using UnityEngine;


public class ObjectsLoader : MonoBehaviour
{
    // Plantation zone
    [SerializeField] private Transform plantationZone;
    public PlantationZones zones;


    // Awake method
    void Awake()
    {
        // Load saved plantation zones
        LoadPlantationZones();
    }

    /// <summary>
    /// Load all saved plantation zones
    /// </summary>
    private void LoadPlantationZones()
    {
        // Getters
        List<Vector3> positions = zones.GetPositions();
        List<Plant> plant = zones.GetPlants();
        List<int> plantingDays = zones.GetPlantingDays();
        List<int> plantingHours = zones.GetPlantingHours();

        // Load all objects saved at zones variable
        for(int i = 0; i < plant.Count; i++)
        {
            // Instantiate the plantation zone
            Transform instance = Instantiate(plantationZone);

            // Enable FollowMouse component
            instance.GetComponent<FollowMouse>().enabled = false;

            // Set position
            instance.position = positions[i];

            // Get PlantationController component and make the initial setup
            PlantationController controller = instance.GetComponent<PlantationController>();
            controller.SetTimeAndId(plantingDays[i], plantingHours[i], i);
            controller.InitialSetup();

            // Setup plant, if there was any
            if(plant[i] != null)
            {
                controller.Plant(plant[i]);
            }
        }
    }
}
