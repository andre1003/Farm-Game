using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Singleton
    public static InventoryUI instance;
    private void Awake()
    {
        if(instance != null)
        {
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
    private Inventory inventory;
    private InventorySlot[] slots;

    // Tab controller
    private bool harvested = false;


    // Start is called before the first frame update
    void Start()
    {
        // Get inventory and add UpdateUI method to On Item Change Callback delegate
        inventory = Inventory.instance;
        inventory.onItemChangeCallback += UpdateUI;

        // Get slots
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        // Update UI
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        // If player press I ky and isBusy is false, change inventory visibility and update UI
        if(Input.GetKeyDown(KeyCode.I) && !InGameSaves.GetIsBusy())
        {
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
            UpdateUI();
        }
    }

    /// <summary>
    /// Method for update inventory UI.
    /// </summary>
    private void UpdateUI()
    {
        // Loop slots
        for(int i = 0; i < slots.Length; i++)
        {
            // If its the correct inventory tab, add it to slot
            if(i < inventory.inventory.plants.Count && inventory.inventory.plants[i].harvested == harvested)
            {
                slots[i].AddPlant(inventory.inventory.plants[i].plant, inventory.inventory.plants[i].amount);
            }

            // Else, clear slot
            else
            {
                slots[i].ClearSlot();
            } 
        }
    }

    /// <summary>
    /// Set harvested variable value.
    /// </summary>
    /// <param name="harvested">Harvasted value.</param>
    public void SetHarvested(bool harvested)
    {
        this.harvested = harvested;
        UpdateUI();
    }

    /// <summary>
    /// Get harvested variable value.
    /// </summary>
    /// <returns>Harvested value.</returns>
    public bool GetHarvested()
    {
        return harvested;
    }
}
