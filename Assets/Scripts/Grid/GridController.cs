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

                if(onMousePrefab == null && enabled) {
                    onMousePrefab = Instantiate(objectToPlace, mousePosition, Quaternion.identity);
                    onMousePrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
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

        float realI = spawn.x;
        float realJ = spawn.z;

        for(int i = 0; i < width; i++, realI+=3) {
            for(int j = 0; j < height; j++, realJ+=3) {
                Vector3 worldPosition = new Vector3(realI, 0, realJ);
                Transform obj = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);
                obj.GetComponent<MeshCollider>().enabled = false;
                obj.name = "Cell_" + name;
                nodes[i, j] = new Node(true, worldPosition, obj);
                name++;
            }
            realJ = spawn.z;
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
                    node.obj.gameObject.GetComponent<Renderer>().material = canPlaceMaterial;

                    if(Input.GetKeyDown(KeyCode.O) && onMousePrefab != null) {
                        Debug.Log("PLACING OBJECT");
                        node.isPlaceable = false;
                        onMousePrefab.GetComponent<FollowMouse>().isOnGrid = true;
                        onMousePrefab.position = node.cellPosition;
                        onMousePrefab.transform.localScale = Vector3.one;
                        onMousePrefab = null;

                        onMousePrefab = Instantiate(objectToPlace, mousePosition, Quaternion.identity);
                        onMousePrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    }
                }
                else {
                    node.obj.gameObject.GetComponent<Renderer>().material = gridMaterial;
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