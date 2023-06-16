using UnityEngine;


public class FollowMouse : MonoBehaviour
{
    // Is on grid?
    public bool isOnGrid;


    // Grid controller reference
    private GridController placeObjectOnGrid;


    // Start is called before the first frame update
    void Start()
    {
        // Get grid controller
        placeObjectOnGrid = FindObjectOfType<GridController>();
    }

    // Update is called once per frame
    void Update()
    {
        // If this is not on grid, follow mouse
        if(!isOnGrid)
        {
            transform.position = placeObjectOnGrid.smoothMousePosition + new Vector3(0, 1f, 0);
        }

    }

    /// <summary>
    /// Costume destroy this object.
    /// </summary>
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
