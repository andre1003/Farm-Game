using UnityEngine;

public class InventoryUI : MonoBehaviour {
    #region Singleton
    public static InventoryUI instance;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of InventoryUI found!");
            return;
        }

        instance = this;
    }

    #endregion

    public Transform itemsParent;
    public GameObject inventoryCanvas;
    
    Inventory inventory;
    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start() {
        inventory = Inventory.instance;
        inventory.onItemChangeCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        UpdateUI();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.I) && !InGameSaves.GetIsBusy()) {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
            UpdateUI();
        }
    }

    // Method for update inventory UI
    private void UpdateUI() {
        for(int i = 0; i < slots.Length; i++) {
            if(i < inventory.inventory.plants.Count)
                slots[i].AddPlant(inventory.inventory.plants[i].plant, inventory.inventory.plants[i].amount);
            else
                slots[i].ClearSlot();
        }
    }
}
