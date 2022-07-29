using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantationZones", menuName = "Loader/PlantationZones")]
public class PlantationZones : ScriptableObject {
    public List<Vector3> positions;
    public List<Plant> plants;
    public List<int> times;

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

    public void RemovePlant(int index) {
        plants.RemoveAt(index);
    }

    public int AddTime(int time, int index) {
        if(index == -1) {
            times.Add(time);
            return times.Count - 1;
        }
        else {
            times[index] = time;
            return index;
        }
    }

    public List<int> GetTimes() {
        return times;
    }
}
