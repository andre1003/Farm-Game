using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ObjectsManager : MonoBehaviour {
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

    // Time
    public TextMeshProUGUI time;
    public int hour = 12;


    // Time settings
    private float clockSeconds;


    void Awake() {
        Instantiate(player, spawnSpot.transform.position, Quaternion.Euler(0, 90, 0));
        clockSeconds = InGameSaves.GetClockSeconds();
    }

    // Start is called before the first frame update
    void Start() {
        Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
        time.text += hour.ToString("00");
    }

    // Update is called once per frame
    void Update() {
        time.text = time.text.Split(' ')[0] + " " + hour.ToString("00");
        clockSeconds -= Time.deltaTime;

        if(clockSeconds < 0) {
            clockSeconds = InGameSaves.GetClockSeconds();
            hour++;
            
            if(hour > 23)
                hour = 0;
        }

        if(EventSystem.current.IsPointerOverGameObject()) {
            Cursor.SetCursor(normalCursor, Vector3.zero, CursorMode.Auto);
            return;
        }

        CheckCursorChange();
    }

    /// <summary>
    /// Check if cursor needs to be changed
    /// </summary>
    private void CheckCursorChange() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)) {
            if(hit.collider.gameObject.tag == "Store") {                                        // Store
                Cursor.SetCursor(buyCursor, Vector3.zero, CursorMode.Auto);
            }
            else if(hit.collider.gameObject.tag == "Workbench") {                               // Workbench
                Cursor.SetCursor(craftCursor, Vector3.zero, CursorMode.Auto);
            }
            else if(hit.collider.gameObject.tag == "Plantable") {                               // Plantation zone
                Cursor.SetCursor(plantCursor, Vector3.zero, CursorMode.Auto);
                if(Input.GetButtonDown("Fire2") && !PlayerDataManager.instance.GetEditMode()) { // If Fire2 at a plantation zone
                    PlantationController controller = hit.collider.gameObject.GetComponent<PlantationController>();

                    // If there is a plant on this platation zone
                    if(controller.HasPlant()) {
                        bool canHarvest = controller.CanHarvest();
                        bool isRotten = controller.IsRotten();

                        if(canHarvest && !isRotten)
                            controller.Harvest();
                        else
                            controller.DestroyPlants();
                    }
                    else
                        inventory.SetActive(true);
                    
                    InGameSaves.SetPlantationZone(hit.collider.gameObject);
                }
                    
            }
            else {
                Cursor.SetCursor(normalCursor, Vector3.zero, CursorMode.Auto);
            }
        }
    }
}
