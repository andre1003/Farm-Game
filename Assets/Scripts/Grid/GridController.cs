using System.Collections.Generic;
using UnityEngine;


public class GridController : MonoBehaviour {
    // Spawn vector
    public Vector3 spawn;

    // Grid materials
    public Material gridMaterial;
    public Material canPlaceMaterial;
    public Material canNotPlaceMaterial;

    // Grid prefab and object to be placed
    public Transform gridCellPrefab;
    public List<Transform> grassPrefab;
    [SerializeField] private Transform objectToPlace;

    // Nodes matrix
    public Node[,] nodes;

    // Mouse prefab and smooth mouse position
    public Transform onMousePrefab;
    public Vector3 smoothMousePosition;

    // Plantation zone scriptable object
    public PlantationZones zones;

    // Grid height and width
    [SerializeField] private int height;
    [SerializeField] private int width;

    // Ground mesh renderer
    public MeshRenderer groundMeshRenderer;

    // Current mouse position
    private Vector3 mousePosition;




    // THIS IS A WORK IN PROGRESS TO ADD GRASS TO THE FARM
    void Start ()
    {
        // Initial configuration
        int name = 0;
        float realI = spawn.x;
        float realJ = spawn.z;

        // Iterate the 'nodes' matrix
        for(int i = 0; i < width; i++, realI += 3)
        {
            for(int j = 0; j < height; j++, realJ += 3)
            {
                // Instantiate the grid cell object
                Vector3 worldPosition = new Vector3(realI, 0, realJ);
                int index = Random.Range(0, grassPrefab.Count);
                Transform obj = Instantiate(grassPrefab[index], worldPosition, Quaternion.identity);

                // Set the grid cell collision to false
                obj.GetComponent<MeshCollider>().enabled = false;

                // Set the grid cell name
                obj.name = "Grass_" + name;

                // Increase the name auxiliar variable
                name++;
            }

            // Set the real column to spawn.z
            realJ = spawn.z;
        }
    }
    




    // Update is called once per frame
    void Update() {
        // If player hits P key
        if(Input.GetKeyDown(KeyCode.P)) {
            // If player is not on edit mode
            if(!PlayerDataManager.instance.GetEditMode()) {
                // Create the grid and set player data to edit mode
                CreateGrid();
                PlayerDataManager.instance.SetEditMode(true);

                // If there is not a plantation zone attached to mouse, create it
                if(onMousePrefab == null && enabled) {
                    InstantiateObjectOnMouse();
                }
            }

            // If player IS on edit mode
            else {
                // Destroy the grid and set the player data to edit mode to false
                DestroyGrid();
                PlayerDataManager.instance.SetEditMode(false);

                // Call plantation zone FollowMouse.DestroyMe() and set it to null
                onMousePrefab.GetComponent<FollowMouse>().DestroyMe();
                onMousePrefab = null;
            }

        }

        // If player is on edit mode, check the mouse position over the grid
        if(PlayerDataManager.instance.GetEditMode())
            GetMousePostitionOnGrid();
    }
    
    /// <summary>
    /// Create edit mode grid when player press P key.
    /// </summary>
    public void CreateGrid() {
        // Create nodes
        nodes = new Node[width, height];

        // Initial configuration
        int name = 0;
        float realI = spawn.x;
        float realJ = spawn.z;

        // Iterate the 'nodes' matrix
        for(int i = 0; i < width; i++, realI += 3) {
            for(int j = 0; j < height; j++, realJ += 3) {
                // Instantiate the grid cell object
                Vector3 worldPosition = new Vector3(realI, 0, realJ);
                Transform obj = Instantiate(gridCellPrefab, worldPosition, Quaternion.identity);

                // Set the grid cell collision to false
                obj.GetComponent<MeshCollider>().enabled = false;

                // Set the grid cell name
                obj.name = "Cell_" + name;

                // Add the grid cell to nodes
                nodes[i, j] = new Node(true, worldPosition, obj);

                // Increase the name auxiliar variable
                name++;
            }

            // Set the real column to spawn.z
            realJ = spawn.z;
        }

        // After creating the grid, check for any objects that need to be loaded
        CheckLoadedObjects();
    }

    /// <summary>
    /// Check for objects that need to be loaded on grid.
    /// </summary>
    private void CheckLoadedObjects() {
        // Get all plantation zones positions that need to be loaded
        List<Vector3> positions = zones.GetPositions();

        // Create the plantation zone at the corresponding grid cell
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

        // Check mouse hover hit event
        if(Physics.Raycast(ray, out hit)) {
            if(hit.transform.gameObject.tag == "Cell" || hit.transform.gameObject.tag == "Plantable") {
                smoothMousePosition = hit.transform.position;

                // Iterate all grid cell nodes
                foreach(Node node in nodes) {
                    // If this is the cell that player's mouse is over and this cell is placeable
                    if(node.cellPosition == hit.transform.position && node.isPlaceable) {
                        // Change the grid cell color to green (canPlaceMaterial)
                        node.obj.gameObject.GetComponent<Renderer>().material = canPlaceMaterial;

                        // If player hits O key or right mouse button, place the plantation zone on this grid cell
                        if(Input.GetKeyDown(KeyCode.O) || Input.GetButtonDown("Fire2") && onMousePrefab != null) {
                            // Configure the plantation zone prefab
                            node.isPlaceable = false;
                            onMousePrefab.GetComponent<FollowMouse>().isOnGrid = true;
                            onMousePrefab.position = node.cellPosition;
                            onMousePrefab.GetComponent<BoxCollider>().enabled = true;
                            
                            // Try to configure plantation zone material and time
                            try {
                                // Change plantation zone material
                                foreach(var renderer in onMousePrefab.GetComponentsInChildren<Renderer>())
                                {
                                    Color materialColor = renderer.material.color;
                                    renderer.material.color = new Color(materialColor.r, materialColor.g, materialColor.b, 1f);
                                }

                                // Change plantation zone time to 0
                                onMousePrefab.gameObject.GetComponent<PlantationController>().InitialSetup();

                            }
                            catch { }

                            // Save at scriptable object
                            zones.AddZone(onMousePrefab.position, null);
                            onMousePrefab = null;

                            // Add another plantation zone prefab attached to mouse
                            InstantiateObjectOnMouse();
                        }
                    }

                    // If player's mouse is over this grid cell, but the cell is NOT placeable
                    else if(node.cellPosition == hit.transform.position && !node.isPlaceable) {
                        // Change grid cell color to red (canNotPlaceMaterial)
                        node.obj.gameObject.GetComponent<Renderer>().material = canNotPlaceMaterial;

                        // If player clicks, remove the plantation zone at this grid cell
                        if(Input.GetKeyDown(KeyCode.O) || Input.GetButtonDown("Fire2")) {
                            node.isPlaceable = true;
                            zones.RemoveZone(node.cellPosition);

                            try {
                                hit.transform.gameObject.GetComponent<PlantationController>().DestroyMe();
                            }
                            catch { }

                            Destroy(hit.transform.gameObject);
                        }
                    }

                    // If this cell is not being hovered, just apply the default grid material
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
        // Instatiate the plantation zone prefab
        onMousePrefab = Instantiate(objectToPlace, mousePosition, Quaternion.identity);

        // Remove collision and attach to mouse
        onMousePrefab.GetComponent<BoxCollider>().enabled = false;
        onMousePrefab.GetComponent<FollowMouse>().enabled = true;

        // Set the object material alpha to 0.5f
        foreach(var renderer in onMousePrefab.GetComponentsInChildren<Renderer>())
        {
            Color materialColor = renderer.material.color;
            renderer.material.color = new Color(materialColor.r, materialColor.g, materialColor.b, 0.5f);
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