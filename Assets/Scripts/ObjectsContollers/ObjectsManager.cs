using UnityEngine;


public class ObjectsManager : MonoBehaviour
{
    // Player spawn settings
    public Transform player;
    public GameObject spawnSpot;
    public GameObject inventory;

    // Cursors
    public Texture2D hoverCursor;
    public Texture2D buyCursor;
    public Texture2D craftCursor;
    public Texture2D plantCursor;

    // Hot spot
    public Vector2 hotSpot = Vector2.zero;


    // Hovering UI controller
    private bool isHoveringUI = false;


    #region Singleton
    // Instance
    public static ObjectsManager instance;

    // Awake method
    void Awake()
    {
        Instantiate(player, spawnSpot.transform.position, Quaternion.Euler(0, 90, 0));

        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion


    // Update is called once per frame
    void Update()
    {
        // If player is hovering an UI, exit
        if(isHoveringUI)
        {
            return;
        }

        // Check for any cursor change
        CheckCursorChange();
    }

    /// <summary>
    /// Check if cursor needs to be changed.
    /// </summary>
    private void CheckCursorChange()
    {
        // Get ray from mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Define tag string
        string tag = "";

        // If the ray hits something
        if(Physics.Raycast(ray, out hit))
        {
            tag = hit.collider.gameObject.tag;
        }

        // If tag is null, exit
        if(tag == "")
        {
            return;
        }

        // If tag is Untagged, set cursor to null and exit
        if(tag == "Untagged")
        {
            Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
            return;
        }

        // Update cursor
        UpdateCursor(hit.collider.gameObject);
    }

    /// <summary>
    /// Update cursor, based on a hit object.
    /// </summary>
    /// <param name="hitObject">Hit object reference.</param>
    private void UpdateCursor(GameObject hitObject)
    {
        // Switch hit object tag
        switch(hitObject.tag)
        {
            case "Store": // Store
                Cursor.SetCursor(buyCursor, Vector3.zero, CursorMode.Auto);
                break;

            case "Workbench": // Workbench
                Cursor.SetCursor(craftCursor, Vector3.zero, CursorMode.Auto);
                break;

            case "Plantable": // Plantable
                Cursor.SetCursor(plantCursor, Vector3.zero, CursorMode.Auto);
                StartPlantingOrHarvest(hitObject);
                break;

            default: // Default
                Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
                break;
        }
    }

    /// <summary>
    /// Start planting or harvest.
    /// </summary>
    /// <param name="hitObject">Hit game object reference.</param>
    private void StartPlantingOrHarvest(GameObject hitObject)
    {
        // If Fire2 at a plantation zone and player is not in edit mode
        if(Input.GetButtonDown("Fire2") && !PlayerDataManager.instance.GetEditMode())
        {
            // Get PlantationController component
            PlantationController controller = hitObject.GetComponent<PlantationController>();

            // If there is a plant on this platation zone
            if(controller.HasPlant())
            {
                // Check if can harvest and if it is rotten
                bool canHarvest = controller.CanHarvest();
                bool isRotten = controller.IsRotten();

                // If the plant can be harvested and it is not rotten, harvest
                if(canHarvest && !isRotten)
                {
                    controller.Harvest();
                }

                // If it is rotten or not ready to harvert, destroy it
                else
                {
                    controller.DestroyPlants();
                }
            }

            // If there is no plant planted, open inventory
            else
            {
                InventoryUI.instance.SetUI(true);
                InventoryUI.instance.SetTab(0);
                HUDManager.instance.SetHUD(false);
            }

            // Set current plantation zone
            InGameSaves.SetPlantationZone(hitObject);
        }
    }

    /// <summary>
    /// Start hovering UI.
    /// </summary>
    public void HoverUI()
    {
        Cursor.SetCursor(hoverCursor, Vector3.zero, CursorMode.Auto);
        isHoveringUI = true;
    }

    /// <summary>
    /// Stop hovering UI.
    /// </summary>
    public void StopHoverUI()
    {
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
        isHoveringUI = false;
    }
}
