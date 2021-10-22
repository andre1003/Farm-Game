using UnityEngine;

public class InventoryUI : MonoBehaviour {
    public Transform itemsParent;
    public GameObject inventoryCanvas;
    
    Inventory inventory;
    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start() {
        inventory = Inventory.instance;
        inventory.onItemChangeCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.I)) {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        }
    }

    private void UpdateUI() {
        for(int i = 0; i < slots.Length; i++) {
            if(i < inventory.plants.Count)
                slots[i].AddPlant(inventory.plants[i]);
            else
                slots[i].ClearSlot();
        }
    }
}
