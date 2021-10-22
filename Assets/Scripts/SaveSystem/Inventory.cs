using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    #region Singleton

    public delegate void OnItemChange();
    public OnItemChange onItemChangeCallback;

    public static Inventory instance;

    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }
    #endregion

    public List<Plant> plants = new List<Plant>();
    public int slots = 10;

    public bool Add(Plant plant) {
        if(plants.Count >= slots) {
            Debug.Log("No slots available!");
            return false;
        }
        else {
            plants.Add(plant);

            if(onItemChangeCallback != null)
                onItemChangeCallback.Invoke();

            return true;
        }
            
    }

    public void Remove(Plant plant) {
        plants.Remove(plant);

        if(onItemChangeCallback != null)
            onItemChangeCallback.Invoke();
    }
}
