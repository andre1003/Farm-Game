using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectsManager : MonoBehaviour {
    public GameObject spawnableObject;
    public GameObject spawnSpot;
    public GameObject inventory;

    public Texture2D normalCursor;
    public Texture2D buyCursor;
    public Texture2D craftCursor;
    public Texture2D plantCursor;

    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start() {
        Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update() {
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
