using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewPlantationZones", menuName = "Loader/PlantationZones")]
public class PlantationZones : ScriptableObject
{
    // Positions
    public List<Vector3> positions;

    // Plants
    public List<Plant> plants;

    // Times
    public List<int> times;


    /// <summary>
    /// Add a zone to scriptable object.
    /// </summary>
    /// <param name="position">Plantation zone position.</param>
    /// <param name="plant">Plant reference.</param>
    public void AddZone(Vector3 position, Plant plant)
    {
        positions.Add(position);
        plants.Add(plant);
    }

    /// <summary>
    /// Remove a zone from scriptable object.
    /// </summary>
    /// <param name="position">Plantation zone position.</param>
    public void RemoveZone(Vector3 position)
    {
        int index = positions.IndexOf(position);
        positions.Remove(position);
        RemovePlant(index);
        RemoveTime(index);
    }

    /// <summary>
    /// Get positions list.
    /// </summary>
    /// <returns>Positions list.</returns>
    public List<Vector3> GetPositions()
    {
        return positions;
    }

    /// <summary>
    /// Get plants list.
    /// </summary>
    /// <returns>Plants list.</returns>
    public List<Plant> GetPlants()
    {
        return plants;
    }

    /// <summary>
    /// Set a plant to a zone.
    /// </summary>
    /// <param name="position">Plantation zone position.</param>
    /// <param name="plant">Plant reference.</param>
    public void SetPlant(Vector3 position, Plant plant)
    {
        int index = positions.IndexOf(position);
        plants[index] = plant;
    }

    /// <summary>
    /// Remove a plant from list.
    /// </summary>
    /// <param name="index">List index to remove.</param>
    public void RemovePlant(int index)
    {
        plants.RemoveAt(index);
    }

    /// <summary>
    /// Add time to list.
    /// </summary>
    /// <param name="time">Time to add.</param>
    /// <param name="index">Index to add.</param>
    /// <returns>Return the index of added time.</returns>
    public int AddTime(int time, int index)
    {
        if(index == -1)
        {
            times.Add(time);
            return times.Count - 1;
        }
        else
        {
            times[index] = time;
            return index;
        }
    }

    /// <summary>
    /// Remove time from list.
    /// </summary>
    /// <param name="index">Index to remove.</param>
    public void RemoveTime(int index)
    {
        try
        {
            times.RemoveAt(index);
        }
        catch { }
    }

    /// <summary>
    /// Get time list.
    /// </summary>
    /// <returns>Time list.</returns>
    public List<int> GetTimes()
    {
        return times;
    }

    /// <summary>
    /// Update IDs.
    /// </summary>
    /// <param name="index">Idenx to start updating.</param>
    private void UpdateIds(int index)
    {
        for(int i = index; i < plants.Count; i++)
        {
            UpdateId(positions[i], i - 1);
        }
    }

    /// <summary>
    /// Update ID.
    /// </summary>
    /// <param name="position">Plantation zone position.</param>
    /// <param name="newId">New ID.</param>
    private void UpdateId(Vector3 position, int newId)
    {
        Collider[] colliders;

        // Presuming the object you are testing also has a collider 0 otherwise
        if((colliders = Physics.OverlapSphere(position, 1f)).Length > 1)
        {
            foreach(var collider in colliders)
            {
                collider.gameObject.GetComponent<PlantationController>().SetId(newId);
            }
        }
    }
}
