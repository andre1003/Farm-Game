using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsLoader : MonoBehaviour {
    [SerializeField] private Transform plantationZone;
    public PlantationZones zones;

    void Awake() {
        LoadPlantationZones();
    }

    // Update is called once per frame
    void Update() {

    }

    private void LoadPlantationZones() {
        // Getters
        List<Vector3> positions = zones.GetPositions();
        List<Plant> plant = zones.GetPlants();
        List<int> times = zones.GetTimes();
        
        // Load all objects saved at zones variable
        for (int i = 0; i < plant.Count; i++) {
            print(positions[i]);
            Transform instance = Instantiate(plantationZone);
            instance.GetComponent<FollowMouse>().enabled = false;
            instance.position = positions[i];

            // Setup plant, if there was any
            if(plant[i] != null) {
                PlantationController controller = instance.GetComponent<PlantationController>();
                controller.Plant(plant[i]);
                controller.SetTimeAndId(times[i], i);
            }
        }
    }
}
