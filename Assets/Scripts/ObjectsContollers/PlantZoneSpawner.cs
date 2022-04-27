using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantZoneSpawner : MonoBehaviour {
    public GameObject plantationZonePrefab;

    public LayerMask groundMask;
    public LayerMask fenceMask;

    public float groundDistance = 0.4f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) {
                if(hit.collider.gameObject.tag != "Plantable")
                    Instantiate(plantationZonePrefab, hit.point, Quaternion.identity);
                
                else {
                    Debug.Log("This Game Object is a Platation Zone!\nX: " + hit.collider.gameObject.transform.position.x + " Y: " + hit.collider.gameObject.transform.position.y + " Z: " + hit.collider.gameObject.transform.position.z);

                    // Its getting the correct location for the platation zone. Now we just have to spawn it correctly
                    Vector3 location = new Vector3(hit.collider.gameObject.transform.position.x + 5, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z);
                    Vector3 location2 = new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, hit.collider.gameObject.transform.position.z + 5);
                    bool canSpawn = Physics.CheckSphere(location, 5f, fenceMask);

                    // It doesn't place at left nor back
                    if(!canSpawn) {
                        if(location.x - hit.point.x < location2.z - hit.point.z)
                            Instantiate(plantationZonePrefab, location, Quaternion.identity);
                        else
                            Instantiate(plantationZonePrefab, location2, Quaternion.identity);
                    }                    
                }
            }
        }
    }

    // Not working. Needs more research
    private bool PreventSpawnOverlap(Vector3 position) {
        Collider[] colliders = Physics.OverlapSphere(position, groundDistance, groundMask);

        for(int i = 0; i < colliders.Length; i++) {
            Vector3 center = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x;
            float height = colliders[i].bounds.extents.z;

            float leftExtent = center.x - width;
            float rightExtent = center.x + width;
            float lowerExtent = center.z - height;
            float upperExtent = center.z + height;

            if(position.x >= leftExtent && position.x <= rightExtent) {
                if(position.z >= lowerExtent && position.z <= upperExtent) {
                    return false;
                }
            }


        }
        return true;
    }
}
