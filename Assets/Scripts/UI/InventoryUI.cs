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

    // UI elements references
    public Transform itemsParent;
    public GameObject inventoryCanvas;
    
    // Inventory references
    Inventory inventory;
    InventorySlot[] slots;

    // Tab controller
    private bool harvested = false;

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
            if(i < inventory.inventory.plants.Count && inventory.inventory.plants[i].harvested == harvested)
                slots[i].AddPlant(inventory.inventory.plants[i].plant, inventory.inventory.plants[i].amount);
            else
                slots[i].ClearSlot();
        }
    }

    // Set harvested variable value
    public void SetHarvested(bool harvested) {
        this.harvested = harvested;

        UpdateUI();
    }

    // Get harvested variable value
    public bool GetHarvested() {
        return harvested;
    }
}
