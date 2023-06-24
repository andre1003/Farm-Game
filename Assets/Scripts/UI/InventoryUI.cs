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

    // Transition
    public float transitionLenght = 0.25f;


    // Inventory references
    private Inventory inventory;
    private InventorySlot[] slots;

    // Tab controller
    private bool harvested = false;

    // UI
    private CanvasGroup canvasGroup;
    private bool changeUI = false;

    // Transition
    private float initialAlpha;
    private float targetAlpha;
    private float elapsedTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        // Get inventory and add UpdateUI method to On Item Change Callback delegate
        inventory = Inventory.instance;
        inventory.onItemChangeCallback += UpdateUI;

        // Get slots
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        // Get canvas group
        canvasGroup = inventoryCanvas.GetComponent<CanvasGroup>();

        // Update UI
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        // If player press I ky and isBusy is false, change inventory visibility and update UI
        if(Input.GetKeyDown(KeyCode.I) && !InGameSaves.GetIsBusy())
        {
            HUDManager.instance.SetHUD(inventoryCanvas.activeSelf);
            SetUI(!inventoryCanvas.activeSelf);
            UpdateUI();
        }

        // If doesn't need to change HUD visibility, exit
        if(!changeUI)
        {
            return;
        }

        // Change UI opacity over transition lenght seconds
        canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, (elapsedTime / transitionLenght));
        elapsedTime += Time.deltaTime;

        // If UI opacity is the target opacity, reset elapsed time and set change UI controller to false
        if(canvasGroup.alpha == targetAlpha)
        {
            elapsedTime = 0f;
            changeUI = false;

            // If target opacity is 0, set disable store canvas
            if(targetAlpha == 0f)
            {
                inventoryCanvas.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set UI visibility.
    /// </summary>
    /// <param name="isActive">New visibility.</param>
    private void SetUI(bool isActive)
    {
        // Set initial opacity
        initialAlpha = canvasGroup.alpha;

        // If the target is to display UI, set target opacity to 1
        if(isActive)
        {
            targetAlpha = 1f;
            inventoryCanvas.SetActive(true);
        }

        // If the target is to hide UI, set target opacity to 0
        else
        {
            targetAlpha = 0f;
        }

        // Set change UI controller to true
        changeUI = true;
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
