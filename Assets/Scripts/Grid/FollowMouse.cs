using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {
    public bool isOnGrid;


    private GridController placeObjectOnGrid;


    // Start is called before the first frame update
    void Start() {
        placeObjectOnGrid = FindObjectOfType<GridController>();
    }

    // Update is called once per frame
    void Update() {
        if(!isOnGrid) {
            transform.position = placeObjectOnGrid.smoothMousePosition + new Vector3(0, 1f, 0);
        }

    }

    public void DestroyMe() {
        Destroy(gameObject);
    }
}
