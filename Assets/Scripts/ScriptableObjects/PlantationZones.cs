using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantationZones", menuName = "Loader/PlantationZones")]
public class PlantationZones : ScriptableObject {
    public List<Vector3> positions;
    public List<Plant> plants;

    public void AddZone(Vector3 position, Plant plant) { 
        positions.Add(position);
        plants.Add(plant);
    }

    public void RemoveZone(Vector3 position) {
        int index = positions.IndexOf(position);
        positions.Remove(position);
        plants.RemoveAt(index);
    }

    public List<Vector3> GetPositions() {
        return positions;
    }

    public List<Plant> GetPlants() {
        return plants;
    }

    public void SetPlant(Vector3 position, Plant plant) {
        int index = positions.IndexOf(position);
        plants[index] = plant;
    }
}
