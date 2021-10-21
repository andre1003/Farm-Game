using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsManager : MonoBehaviour {
    public GameObject spawnableObject;
    public GameObject spawnSpot;

    public Texture2D normalCursor;
    public Texture2D buyCursor;
    public Texture2D craftCursor;

    public Vector2 hotSpot = Vector2.zero;

    public PlayerData playerData;

    public Text moneyText;

    private void Awake() {
        PlayerDataManager.SetPlayerData(playerData);
        moneyText.text = PlayerDataManager.GetMoney();
    }

    // Start is called before the first frame update
    void Start() {
        Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.S)) {
            Instantiate(spawnableObject, spawnSpot.transform.position, spawnSpot.transform.rotation);
        }
        if(!InGameSaves.GetIsBusy()) {
            CheckCursorChange();
        }
    }

    // Method for checking is cursor needs to change
    private void CheckCursorChange() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)) {
            if(hit.collider.gameObject.tag == "Store") {
                Cursor.SetCursor(buyCursor, Vector3.zero, CursorMode.Auto);
            }
            else if(hit.collider.gameObject.tag == "Workbench") {
                Cursor.SetCursor(craftCursor, Vector3.zero, CursorMode.Auto);
            }
            else {
                Cursor.SetCursor(normalCursor, Vector3.zero, CursorMode.Auto);
            }
        }
    }
}
