using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {
    public Vector3 spawn;

    public Transform gridCellPrefab;
    [SerializeField] private Transform objectToPlace;

    public Node[,] nodes;

    public Transform onMousePrefab;
    public Vector3 smoothMousePosition;

    [SerializeField] private int height;
    [SerializeField] private int width;

    public MeshRenderer groundMeshRenderer;

    private Plane plane;
    private Vector3 mousePosition;
    private bool isGridCreated = false;

    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            if(!isGridCreated) {
                groundMeshRenderer.enabled = false;
                CreateGrid();
                plane = new Plane(Vector3.up, transform.position);
                isGridCreated = true;

                if(onMousePrefab == null && enabled)
                    onMousePrefab = Instantiate(objectToPlace, mousePosition, Quaternion.identity);
            }
            else {
                groundMeshRenderer.enabled = true;
                DestroyGrid();
                isGridCreated = false;
                onMousePrefab.GetComponent<FollowMouse>().DestroyMe();
                onMousePrefab = null;
            }
                
        }
        if(isGridCreated)
            GetMousePostitionOnGrid();
    }

    public void CreateGrid() {
        nodes = new Node[width, height];
        int name = 0;

        float realI = spawn.x + 1f;
        float realJ = spawn.z + 1f;

        for(int i = 0; i < width; i++, realI++) {
            for(int j = 0; j < height; j++, realJ++) {
                Vector3 worldPosition = new Vector3(realI, 0, realJ);
                Transform obj = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
                obj.GetComponent<MeshCollider>().enabled = false;
                obj.name = "Cell_" + name;
                nodes[i, j] = new Node(true, worldPosition, obj);
                name++;
            }
            realJ = spawn.z + 1f;
        }
    }

    public void DestroyGrid() {
        GameObject[] cells = GameObject.FindGameObjectsWithTag("Cell");
        int length = cells.Length;

        for(int i = 0; i < length; i++)
            Destroy(cells[i]);

        nodes = null;
    }

    private void GetMousePostitionOnGrid() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(plane.Raycast(ray, out var enter)) {
            mousePosition = ray.GetPoint(enter);
            smoothMousePosition = mousePosition;
            mousePosition = Vector3Int.RoundToInt(mousePosition);

            foreach(Node node in nodes) {
                if(node.cellPosition == mousePosition && node.isPlaceable) {
                    if(Input.GetKeyDown(KeyCode.O) && onMousePrefab != null) {
                        Debug.Log("PLACING OBJECT");
                        node.isPlaceable = false;
                        onMousePrefab.GetComponent<FollowMouse>().isOnGrid = true;
                        onMousePrefab.position = node.cellPosition;
                        onMousePrefab = null;

                        onMousePrefab = Instantiate(objectToPlace, mousePosition, Quaternion.identity);
                    }
                }
            }
        }
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