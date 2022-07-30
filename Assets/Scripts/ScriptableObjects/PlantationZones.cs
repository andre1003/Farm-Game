using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlantationZones", menuName = "Loader/PlantationZones")]
public class PlantationZones : ScriptableObject {
    public List<Vector3> positions;
    public List<Plant> plants;
    public List<int> times;

    // Add a zone to scriptable object
    public void AddZone(Vector3 position, Plant plant) {
        positions.Add(position);
        plants.Add(plant);
    }

    // Remove a zone from scriptable object
    public void RemoveZone(Vector3 position) {
        int index = positions.IndexOf(position);
        positions.Remove(position);
        RemovePlant(index);
        RemoveTime(index);
    }

    // Get positions list
    public List<Vector3> GetPositions() {
        return positions;
    }

    // Get plants list
    public List<Plant> GetPlants() {
        return plants;
    }

    // Set a plant to a zone
    public void SetPlant(Vector3 position, Plant plant) {
        int index = positions.IndexOf(position);
        plants[index] = plant;
    }

    // Remove a plant from list
    public void RemovePlant(int index) {
        plants.RemoveAt(index);
    }

    // Add time to list
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

    // Remove time from list
    public void RemoveTime(int index) {
        try {
            times.RemoveAt(index);
        }
        catch { }
    }

    // Get time list
    public List<int> GetTimes() {
        return times;
    }

    private void UpdateIds(int index) {
        for(int i = index; i < plants.Count; i++)
            UpdateId(positions[i], i - 1);
    }

    private void UpdateId(Vector3 position, int newId) {
        Collider[] colliders;

        // Presuming the object you are testing also has a collider 0 otherwise
        if((colliders = Physics.OverlapSphere(position, 1f)).Length > 1)
            foreach(var collider in colliders)
                collider.gameObject.GetComponent<PlantationController>().SetId(newId);
    }
}
