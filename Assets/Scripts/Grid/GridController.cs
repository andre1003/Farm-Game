using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {
    public Vector3 spawn;

    public Material gridMaterial;
    public Material canPlaceMaterial;
    public Material canNotPlaceMaterial;

    public Transform gridCellPrefab;
    [SerializeField] private Transform objectToPlace;

    public Node[,] nodes;

    public Transform onMousePrefab;
    public Vector3 smoothMousePosition;

    public PlantationZones zones;

    [SerializeField] private int height;
    [SerializeField] private int width;

    public MeshRenderer groundMeshRenderer;

    private Plane plane;
    private Vector3 mousePosition;
    //private bool isGridCreated = false;


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            if(!PlayerDataManager.instance.GetEditMode()) { // CHANGE TO PLAYERDATAMANAGER EDIT MODE VARIABLE
                CreateGrid();
                plane = new Plane(Vector3.up, transform.position);
                PlayerDataManager.instance.SetEditMode(true);

                if(onMousePrefab == null && enabled) {
                    InstantiateObjectOnMouse();
                }
            }
            else {
                DestroyGrid();
                PlayerDataManager.instance.SetEditMode(false);
                onMousePrefab.GetComponent<FollowMouse>().DestroyMe();
                onMousePrefab = null;
            }

        }
        if(PlayerDataManager.instance.GetEditMode())
            GetMousePostitionOnGrid();
    }
    
    /// <summary>
    /// Create edit mode grid when player press P key.
    /// </summary>
    public void CreateGrid() {
        nodes = new Node[width, height];
        int name = 0;

        float realI = spawn.x;
        float realJ = spawn.z;

        for(int i = 0; i < width; i++, realI += 3) {
            for(int j = 0; j < height; j++, realJ += 3) {
                Vector3 worldPosition = new Vector3(realI, 0, realJ);
                Transform obj = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
                obj.GetComponent<MeshCollider>().enabled = false;
                obj.name = "Cell_" + name;
                nodes[i, j] = new Node(true, worldPosition, obj);
                name++;
            }
            realJ = spawn.z;
        }

        CheckLoadedObjects();
    }

    private void CheckLoadedObjects() {
        List<Vector3> positions = zones.GetPositions();

        foreach(var node in nodes) {
            foreach(var position in positions) {
                if(node.cellPosition == position) {
                    node.isPlaceable = false;
                }
            }
        }
    }

    /// <summary>
    /// Destroy edit mode grid when player hit P again.
    /// </summary>
    public void DestroyGrid() {
        GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
        int length = cells.Length;

        for(int i = 0; i < length; i++)
            Destroy(cells[i]);

        nodes = null;
    }

    /// <summary>
    /// Cast a raycast to check where the player's mouse is and place or remove an object at that position.
    /// </summary>
    private void GetMousePostitionOnGrid() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit)) {
            if(hit.transform.gameObject.tag == "Cell" || hit.transform.gameObject.tag == "Plantable") {
                smoothMousePosition = hit.transform.position;

                foreach(Node node in nodes) {
                    if(node.cellPosition == hit.transform.position && node.isPlaceable) {
                        node.obj.gameObject.GetComponent<Renderer>().material = canPlaceMaterial;

                        if(Input.GetKeyDown(KeyCode.O) || Input.GetButtonDown("Fire2") && onMousePrefab != null) {
                            Debug.Log("PLACING OBJECT");
                            node.isPlaceable = false;
                            onMousePrefab.GetComponent<FollowMouse>().isOnGrid = true;
                            onMousePrefab.position = node.cellPosition;
                            onMousePrefab.GetComponent<BoxCollider>().enabled = true;

                            // Save at scriptable object
                            zones.AddZone(onMousePrefab.position, null);
                            onMousePrefab = null;

                            InstantiateObjectOnMouse();
                        }
                    }
                    else if(node.cellPosition == hit.transform.position && !node.isPlaceable) {
                        node.obj.gameObject.GetComponent<Renderer>().material = canNotPlaceMaterial;
                        if(Input.GetKeyDown(KeyCode.O) || Input.GetButtonDown("Fire2")) {
                            node.isPlaceable = true;
                            zones.RemoveZone(node.cellPosition);
                            Destroy(hit.transform.gameObject);
                        }
                    }
                    else {
                        node.obj.gameObject.GetComponent<Renderer>().material = gridMaterial;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Instantiates the desirable object on mouse position.
    /// </summary>
    private void InstantiateObjectOnMouse() {
        onMousePrefab = Instantiate(objectToPlace, mousePosition, Quaternion.identity);
        onMousePrefab.GetComponent<BoxCollider>().enabled = false;
        onMousePrefab.GetComponent<FollowMouse>().enabled = true;
    }
}


public class Node {
    public bool isPlaceable;
    public Vector3 cellPosition;
    public Transform obj;

    public Node(bool isPlaceable, Vector3 cellPosition, Transform obj) {
        this.isPlaceable = isPlaceable;
        this.cellPosition = cellPosition;
        this.obj = obj;
    }
}