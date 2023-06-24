using UnityEngine;
using UnityEngine.EventSystems;


public class ObjectsManager : MonoBehaviour
{
    // Player spawn settings
    public Transform player;
    public GameObject spawnSpot;
    public GameObject inventory;

    // Cursors
    public Texture2D normalCursor;
    public Texture2D buyCursor;
    public Texture2D craftCursor;
    public Texture2D plantCursor;

    // Hot spot
    public Vector2 hotSpot = Vector2.zero;


    // Awake method
    void Awake()
    {
        Instantiate(player, spawnSpot.transform.position, Quaternion.Euler(0, 90, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if mouse is over an game object
        if(EventSystem.current.IsPointerOverGameObject())
        {
            // If it is, set cursor to normal
            Cursor.SetCursor(normalCursor, Vector3.zero, CursorMode.Auto);
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

        // If the ray hits something
        if(Physics.Raycast(ray, out hit))
        {
            // Store
            if(hit.collider.gameObject.tag == "Store")
            {
                // Set cursor to buyCursor
                Cursor.SetCursor(buyCursor, Vector3.zero, CursorMode.Auto);
            }

            // Workbench
            else if(hit.collider.gameObject.tag == "Workbench")
            {
                // Set cursor to craftCursor
                Cursor.SetCursor(craftCursor, Vector3.zero, CursorMode.Auto);
            }

            // Plantation zone
            else if(hit.collider.gameObject.tag == "Plantable")
            {
                // Set cursor to plantCursor
                Cursor.SetCursor(plantCursor, Vector3.zero, CursorMode.Auto);

                // If Fire2 at a plantation zone and player is not in edit mode
                if(Input.GetButtonDown("Fire2") && !PlayerDataManager.instance.GetEditMode())
                {
                    // Get PlantationController component
                    PlantationController controller = hit.collider.gameObject.GetComponent<PlantationController>();

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
                        InventoryUI.instance.SetHarvested(false);
                        HUDManager.instance.SetHUD(false);
                    }

                    // Set current plantation zone
                    InGameSaves.SetPlantationZone(hit.collider.gameObject);
                }
            }

            // If it's not an special location, set cursor to normal
            else
            {
                Cursor.SetCursor(normalCursor, Vector3.zero, CursorMode.Auto);
            }
        }
    }
}
