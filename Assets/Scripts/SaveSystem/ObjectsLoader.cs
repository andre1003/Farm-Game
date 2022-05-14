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
        List<Vector3> positions = zones.GetPositions();
        List<Plant> plant = zones.GetPlants();
        

        for (int i = 0; i < plant.Count; i++) {
            print(positions[i]);
            Transform instance = Instantiate(plantationZone);
            instance.GetComponent<FollowMouse>().enabled = false;
            instance.position = positions[i];
            if(plant[i] != null)
                instance.GetComponent<PlantationController>().Plant(plant[i]);
        }
    }
}
