using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour {
    public Transform itemsParent;

    private Store store;
    private StoreSlot[] slots;

    // Start is called before the first frame update
    void Start() {
        store = Store.instance;
        slots = itemsParent.GetComponentsInChildren<StoreSlot>();
        UpdateUI();
    }

    public void UpdateUI() {
        for(int i = 0; i < slots.Length; i++) {
            if(i < store.storePlants.Length)
                slots[i].AddPlant(store.storePlants[i]);
            else
                slots[i].ClearSlot();
        }
    }
}
